using System;

namespace Decongestor.Domain
{
    public class ChargeMatrix
    {
        public int Id { get; set; }

        public TimeSpan FromTimeOfDayInclusive { get; set; }

        public TimeSpan ToTimeOfDayExclusive { get; set; }

        public decimal ChargePerEntry { get; set; }

        public int VehicleTypeId { get; set; }

        public virtual VehicleType VehicleType { get; set; }
    }
}
