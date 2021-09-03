using Decongestor.Domain;
using System;

namespace Decongestor.DataAccess
{
    public interface IChargeCalculatorSourceDataAccess
    {
        Vehicle GetVehicleWithTypeAndLastCharge(string vehicleId);

        decimal GetTotalTollCharge(string vehicleId, DateTime entryDateTimeUtc);

        ChargeMatrix GetChargeMatrix(int vehicleTypeId, DateTime entryDateTimeUtc);

        bool IsHolidayOn(DateTime utcDataTime);
    }
}