using System;

namespace Decongestor.Business
{
    public interface IDateTimeHelper
    {
        DateTime UtcToLocalTime(DateTime utc);
    }
}