using Microsoft.EntityFrameworkCore.Migrations;

namespace Decongestor.DataAccess.Migrations
{
    public partial class TypoFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ChargeMatrix_FromTimeOfDayInclusive_ToTimeOPfDayExclusive",
                table: "ChargeMatrix");

            migrationBuilder.RenameColumn(
                name: "ToTimeOPfDayExclusive",
                table: "ChargeMatrix",
                newName: "ToTimeOfDayExclusive");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ChargeMatrix_FromTimeOfDayInclusive_ToTimeOfDayExclusive",
                table: "ChargeMatrix",
                sql: "(ToTimeOfDayExclusive > FromTimeOfDayInclusive)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ChargeMatrix_FromTimeOfDayInclusive_ToTimeOfDayExclusive",
                table: "ChargeMatrix");

            migrationBuilder.RenameColumn(
                name: "ToTimeOfDayExclusive",
                table: "ChargeMatrix",
                newName: "ToTimeOPfDayExclusive");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ChargeMatrix_FromTimeOfDayInclusive_ToTimeOPfDayExclusive",
                table: "ChargeMatrix",
                sql: "(ToTimeOPfDayExclusive > FromTimeOfDayInclusive)");
        }
    }
}
