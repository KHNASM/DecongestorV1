using System;

namespace Decongestor.Domain
{
    public class TollEntry
    {
        public int Id { get; set; }

        public DateTime EnteredAtUtc { get; set; }

        public DateTime ChargeDate { get; set; }

        public decimal Charge { get; set; }

        public string Remarks { get; set; }

        public string VehicleId { get; set; }

        public virtual Vehicle Vehicle { get; set; }

    }
}
