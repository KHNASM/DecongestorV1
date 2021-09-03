using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Decongestor.DataAccess.Migrations
{
    public partial class ComputedDateColumnOnTollEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ChargeDate",
                table: "TollEntries",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "cast(EnteredAtUtc as date)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChargeDate",
                table: "TollEntries");
        }
    }
}
