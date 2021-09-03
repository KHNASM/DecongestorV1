using System;

namespace Decongestor.Business
{
    public interface ITollChargeCalculator
    {
        TollCharge CalculateCharge(string vehicleId, DateTime entryDateTimeUtc);
    }
}