using Decongestor.DataAccess.Config;
using Decongestor.Domain;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Decongestor.Business;

namespace Decongestor.DataAccess
{
    public class ChargeCalculatorSourceDataAccess : IChargeCalculatorSourceDataAccess
    {
        private readonly DataContext _dataContext;
        private readonly IDateTimeHelper _dateTimeHelper;

        public ChargeCalculatorSourceDataAccess(DataContext dataContext, IDateTimeHelper dateTimeHelper)
        {
            _dataContext = dataContext;
            _dateTimeHelper = dateTimeHelper;
        }

        public ChargeMatrix GetChargeMatrix(int vehicleTypeId, DateTime entryDateTimeUtc)
        {
            DateTime localTime = _dateTimeHelper.UtcToLocalTime(entryDateTimeUtc);

            TimeSpan timeOfDay = localTime - localTime.Date;

            ChargeMatrix matrix = _dataContext.ChargeMatrix
                .Where(cm => cm.VehicleTypeId == vehicleTypeId)
                .Where(cm => timeOfDay >= cm.FromTimeOfDayInclusive && timeOfDay < cm.ToTimeOfDayExclusive)
                .OrderByDescending(cm => cm.ChargePerEntry)
                .FirstOrDefault();

            return matrix;
        }

        public decimal GetTotalTollCharge(string vehicleId, DateTime entryDateTimeUtc)
        {
            DateTime entryDateTime = _dateTimeHelper.UtcToLocalTime(entryDateTimeUtc).Date;

            var totalChargeForTheDay = _dataContext.TollEntries
                .Where(e => e.VehicleId == vehicleId && e.ChargeDate == entryDateTime)
                .Sum(e => (decimal?)e.Charge);

            return totalChargeForTheDay ?? 0;
        }

        public Vehicle GetVehicleWithTypeAndLastCharge(string vehicleId)
        {
            Vehicle vehicle = _dataContext.Vehicles
                .Include(v => v.VehicleType)
                .SingleOrDefault(v => v.Id == vehicleId);

            if (vehicle == null)
            {
                return null;
            }

            TollEntry lastCharged = _dataContext.TollEntries
                .Where(e => e.VehicleId == vehicleId && e.Charge > 0)
                .OrderByDescending(e => e.EnteredAtUtc)
                .FirstOrDefault();


            vehicle.TollEntries = lastCharged == null
                ? Array.Empty<TollEntry>()
                : new[] { lastCharged };

            _dataContext.Entry(vehicle).State = EntityState.Detached; // we do not want to track changes on this instance of vehicle. It's for only read-only purpuse

            return vehicle;
        }

        public bool IsHolidayOn(DateTime utcDataTime)
        {
            var dateTime = _dateTimeHelper.UtcToLocalTime(utcDataTime);

            return _dataContext.HolidayConfiguration
                .Any(hc => hc.Day == dateTime.Day && hc.Month == dateTime.Month && (!hc.Year.HasValue || hc.Year == dateTime.Year));
        }
    }
}
