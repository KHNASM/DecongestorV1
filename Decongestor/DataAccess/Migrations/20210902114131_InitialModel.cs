using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Decongestor.DataAccess.Migrations
{
    public partial class InitialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HolidayConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayConfiguration", x => x.Id);
                    table.CheckConstraint("CK_HolidayConfiguration_Day", "(Day between 1 and 31)");
                    table.CheckConstraint("CK_HolidayConfiguration_Month", "(Month between 1 and 12)");
                    table.CheckConstraint("CK_HolidayConfiguration_Year", "(Year is null or Year between 2020 and 9999)");
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DailyChargeCap = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                    table.CheckConstraint("CK_VehicleType_DailyChargeCap", "(DailyChargeCap is null or DailyChargeCap >= 0)");
                });

            migrationBuilder.CreateTable(
                name: "ChargeMatrix",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromTimeOfDayInclusive = table.Column<TimeSpan>(type: "time", nullable: false),
                    ToTimeOPfDayExclusive = table.Column<TimeSpan>(type: "time", nullable: false),
                    ChargePerEntry = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeMatrix", x => x.Id);
                    table.CheckConstraint("CK_ChargeMatrix_FromTimeOfDayInclusive_ToTimeOPfDayExclusive", "(ToTimeOPfDayExclusive > FromTimeOfDayInclusive)");
                    table.CheckConstraint("CK_ChargeMatrix_ChargePerEntry", "(ChargePerEntry >= 0)");
                    table.ForeignKey(
                        name: "FK_ChargeMatrix_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TollEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnteredAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Charge = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TollEntries", x => x.Id);
                    table.CheckConstraint("CK_TollEntry_Charge", "(Charge >= 0)");
                    table.ForeignKey(
                        name: "FK_TollEntries_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChargeMatrix_VehicleTypeId",
                table: "ChargeMatrix",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TollEntries_VehicleId",
                table: "TollEntries",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChargeMatrix");

            migrationBuilder.DropTable(
                name: "HolidayConfiguration");

            migrationBuilder.DropTable(
                name: "TollEntries");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "VehicleTypes");
        }
    }
}
