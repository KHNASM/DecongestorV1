using Decongestor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Decongestor.DataAccess.Config
{
    public class HolidayConfigurationEntityTypeConfig : IEntityTypeConfiguration<HolidayConfiguration>
    {
        public void Configure(EntityTypeBuilder<HolidayConfiguration> builder)
        {
            // Constraint: Limiting number of days for a month
            builder.HasCheckConstraint(
                "CK_HolidayConfiguration_Day",
                "(Day between 1 and 31)");

            // Constraint: Limiting number of months for a year
            builder.HasCheckConstraint(
                "CK_HolidayConfiguration_Month",
                "(Month between 1 and 12)");

            // Constraint: Year can have a null value or between given range
            builder.HasCheckConstraint(
                "CK_HolidayConfiguration_Year",
                "(Year is null or Year between 2020 and 9999)");
        }
    }
}
