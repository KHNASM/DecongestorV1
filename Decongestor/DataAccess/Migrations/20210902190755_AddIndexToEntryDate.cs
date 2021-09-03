using Microsoft.EntityFrameworkCore.Migrations;

namespace Decongestor.DataAccess.Migrations
{
    public partial class AddIndexToEntryDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TollEntries_EnteredAtUtc",
                table: "TollEntries",
                column: "EnteredAtUtc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TollEntries_EnteredAtUtc",
                table: "TollEntries");
        }
    }
}
