using System;

namespace Decongestor.Business
{
    public class DateTimeHelper : IDateTimeHelper
    {
        public DateTime UtcToLocalTime(DateTime utc) => utc.ToLocalTime();
    }
}
