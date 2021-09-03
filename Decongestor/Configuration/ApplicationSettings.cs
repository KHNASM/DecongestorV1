using System;

namespace Decongestor.Configuration
{
    public class ApplicationSettings
    {
        public decimal DefaultDailyChargeCap { get; set; } = 60;

        public decimal DefaultChargePerEntry { get; set; } = 9;

        public TimeSpan ReEntryExemptionPeriod { get; set; } = TimeSpan.FromHours(1);

        public DayOfWeek[] ExemptedWeekDays { get; set; }

        public TollCharge[] DefaultTollCharges { get; set; }
    }
}
