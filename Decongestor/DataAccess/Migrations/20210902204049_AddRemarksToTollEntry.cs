using Microsoft.EntityFrameworkCore.Migrations;

namespace Decongestor.DataAccess.Migrations
{
    public partial class AddRemarksToTollEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "TollEntries",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "TollEntries");
        }
    }
}
