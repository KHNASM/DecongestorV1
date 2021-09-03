using Decongestor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Decongestor.DataAccess.Config
{
    public class VehicleTypeEntityTypeConfig : IEntityTypeConfiguration<VehicleType>
    {
        public void Configure(EntityTypeBuilder<VehicleType> builder)
        {
            builder.Property(v => v.Description)
                .HasMaxLength(64)
                .IsRequired(true);

            builder.Property(v => v.DailyChargeCap)
                .HasColumnType("decimal(5, 2)");

            builder.HasCheckConstraint(
                "CK_VehicleType_DailyChargeCap",
                "(DailyChargeCap is null or DailyChargeCap >= 0)");
        }
    }
}
