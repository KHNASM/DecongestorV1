using Decongestor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Decongestor.DataAccess.Config
{
    public class HolidayConfigurationEntityTypeConfig : IEntityTypeConfiguration<HolidayConfiguration>
    {
        public void Configure(EntityTypeBuilder<HolidayConfiguration> builder)
        {
            builder.HasCheckConstraint(
                "CK_HolidayConfiguration_Day",
                "(Day between 1 and 31)");

            builder.HasCheckConstraint(
                "CK_HolidayConfiguration_Month",
                "(Month between 1 and 12)");

            builder.HasCheckConstraint(
                "CK_HolidayConfiguration_Year",
                "(Year is null or Year between 2020 and 9999)");
        }
    }
}
