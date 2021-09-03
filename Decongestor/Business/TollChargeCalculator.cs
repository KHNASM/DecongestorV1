using Decongestor.Configuration;
using Decongestor.DataAccess;
using Decongestor.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Decongestor.Business
{
    public class TollChargeCalculator : ITollChargeCalculator
    {
        private readonly IChargeCalculatorSourceDataAccess _dataAccess;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ApplicationSettings _applicationSettings;

        public TollChargeCalculator(
            IOptionsMonitor<ApplicationSettings> optionsMonitor,
            IChargeCalculatorSourceDataAccess dataAccess,
            IDateTimeHelper dateTimeHelper)
        {
            _dataAccess = dataAccess;
            _dateTimeHelper = dateTimeHelper;
            _applicationSettings = optionsMonitor.CurrentValue;
        }

        public TollCharge CalculateCharge(string vehicleId, DateTime entryDateTimeUtc)
        {
            var entryDateTimeLocal = _dateTimeHelper.UtcToLocalTime(entryDateTimeUtc);

            var dayOfWeek = entryDateTimeLocal.DayOfWeek;

            if (_applicationSettings.ExemptedWeekDays.Contains(dayOfWeek))
            {
                return ZeroCharge(vehicleId, entryDateTimeUtc, $"{dayOfWeek} is exempted from Toll Charge");
            }

            var isHoliday = _dataAccess.IsHolidayOn(entryDateTimeUtc);

            if (isHoliday)
            {
                return ZeroCharge(vehicleId, entryDateTimeUtc, $"{entryDateTimeLocal:dd/MM/yyyy} is Holiday");
            }

            Vehicle vehicle = _dataAccess.GetVehicleWithTypeAndLastCharge(vehicleId);

            if (vehicle == null)
            {
                return null;
            }

            if (vehicle.VehicleType.DailyChargeCap == 0)
            {
                return ZeroCharge(vehicleId, entryDateTimeUtc, $"{vehicle.VehicleType.Description} is an exempted Vehical Type");
            }

            TollEntry lastCharge = vehicle.TollEntries
                .OrderByDescending(e => e.EnteredAtUtc)
                .FirstOrDefault();

            if (lastCharge != null)
            {
                TimeSpan lastChargedSince = entryDateTimeUtc - lastCharge.EnteredAtUtc;
                var exemptedReEntryPeriod = _applicationSettings.ReEntryExemptionPeriod;

                if (lastChargedSince <= exemptedReEntryPeriod)
                {
                    return ZeroCharge(
                        vehicleId,
                        entryDateTimeUtc,
                        $"Vehicle re-entered within the exempted re-entry time {exemptedReEntryPeriod.Hours:00}:{exemptedReEntryPeriod.Minutes:00}");
                }
            }

            decimal chargedToday = _dataAccess.GetTotalTollCharge(vehicleId, entryDateTimeUtc);

            decimal dailyChargeCap = vehicle.VehicleType.DailyChargeCap ?? _applicationSettings.DefaultDailyChargeCap;

            if (chargedToday >= dailyChargeCap)
            {
                return ZeroCharge(vehicleId, entryDateTimeUtc, $"Daily charge cap of {dailyChargeCap:C} reached");
            }

            ChargeMatrix chargeMatrix = _dataAccess.GetChargeMatrix(vehicle.VehicleTypeId, entryDateTimeUtc);
            var charge = chargeMatrix?.ChargePerEntry;

            if (!charge.HasValue)
            {
                TimeSpan timeOfDay = entryDateTimeLocal - entryDateTimeLocal.Date;

                var configuredTimeOfDayCharge = _applicationSettings.DefaultTollCharges
                    .Where(tc => timeOfDay >= tc.FromTimeOfDayInclusive && timeOfDay < tc.ToTimeOfDayExclusive)
                    .OrderByDescending(tc => tc.Charge)
                    .FirstOrDefault();

                charge = configuredTimeOfDayCharge?.Charge;
            }

            var calculatedCharge = charge ?? _applicationSettings.DefaultChargePerEntry;

            var finalCharge = calculatedCharge;
            var accumulatedChargeForTheDay = chargedToday + calculatedCharge;

            string remarks = null;

            if(accumulatedChargeForTheDay > dailyChargeCap)
            {
                finalCharge = dailyChargeCap - chargedToday;
                remarks = $"Actual charge of {calculatedCharge:C} capped to {finalCharge:C} to keep within daily charge cap of {dailyChargeCap:C}";
            }

            return new TollCharge(vehicleId, entryDateTimeUtc, finalCharge, remarks);
        }

        private TollCharge ZeroCharge(string vehicleId, DateTime entryDateTimeUtc, string remarks)
        {
            return new TollCharge(vehicleId, entryDateTimeUtc, 0, remarks);
        }
    }
}
