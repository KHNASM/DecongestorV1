using Decongestor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Decongestor.DataAccess.Config
{
    public class TollEntryEntityTypeConfig : IEntityTypeConfiguration<TollEntry>
    {
        public void Configure(EntityTypeBuilder<TollEntry> builder)
        {
            builder.HasIndex(e => e.EnteredAtUtc)
                .IsUnique(false);

            builder.Property(e => e.ChargeDate)
                .HasComputedColumnSql("cast(EnteredAtUtc as date)");

            builder.HasCheckConstraint("CK_TollEntry_Charge", "(Charge >= 0)");

            builder.Property(v => v.Charge)
                .HasColumnType("decimal(5, 2)");

        }
    }
}
