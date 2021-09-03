using System;

namespace Decongestor.Business
{
    public class TollCharge
    {
        public TollCharge(string vehicleId, DateTime entryDateTimeUtc, decimal charge, string remarks)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
            {
                throw new ArgumentException($"'{nameof(vehicleId)}' cannot be null or whitespace.", nameof(vehicleId));
            }

            VehicleId = vehicleId;
            EntryDateTimeUtc = entryDateTimeUtc;
            Charge = charge;
            Remarks = remarks;
        }

        public string VehicleId { get; }

        public DateTime EntryDateTimeUtc { get; }

        public decimal Charge { get; }

        public string Remarks { get; }
    }
}
