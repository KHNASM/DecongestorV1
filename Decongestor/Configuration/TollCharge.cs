using System;

namespace Decongestor.Configuration
{
    public class TollCharge
    {
        public TimeSpan FromTimeOfDayInclusive { get; set; }

        public TimeSpan ToTimeOfDayExclusive { get; set; }

        public decimal Charge { get; set; }
    }
}
