using Decongestor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Decongestor.DataAccess.Config
{
    public class ChargeMatrixEntityTypeConfig : IEntityTypeConfiguration<ChargeMatrix>
    {
        public void Configure(EntityTypeBuilder<ChargeMatrix> builder)
        {
            builder.Property(v => v.ChargePerEntry)
                .HasColumnType("decimal(5, 2)");

            builder.HasCheckConstraint(
                "CK_ChargeMatrix_FromTimeOfDayInclusive_ToTimeOfDayExclusive",
                "(ToTimeOfDayExclusive > FromTimeOfDayInclusive)");

            builder.HasCheckConstraint(
               "CK_ChargeMatrix_ChargePerEntry",
               "(ChargePerEntry >= 0)");
        }
    }
}
