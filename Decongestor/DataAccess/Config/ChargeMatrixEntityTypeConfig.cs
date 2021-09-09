using Decongestor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Decongestor.DataAccess.Config
{
    public class ChargeMatrixEntityTypeConfig : IEntityTypeConfiguration<ChargeMatrix>
    {
        public void Configure(EntityTypeBuilder<ChargeMatrix> builder)
        {
            // Column for decimal type, ChargePerEntry
            builder.Property(v => v.ChargePerEntry)
                .HasColumnType("decimal(5, 2)");


            // Constraint: Exclusive time should always be greater than inclusive time
            builder.HasCheckConstraint(
                "CK_ChargeMatrix_FromTimeOfDayInclusive_ToTimeOfDayExclusive",
                "(ToTimeOfDayExclusive > FromTimeOfDayInclusive)");


            // not null constraint
            builder.HasCheckConstraint(
               "CK_ChargeMatrix_ChargePerEntry",
               "(ChargePerEntry >= 0)");
        }
    }
}
