// <auto-generated />
using System;
using Decongestor.DataAccess.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Decongestor.DataAccess.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210902204049_AddRemarksToTollEntry")]
    partial class AddRemarksToTollEntry
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Decongestor.Domain.ChargeMatrix", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("ChargePerEntry")
                        .HasColumnType("decimal(5,2)");

                    b.Property<TimeSpan>("FromTimeOfDayInclusive")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("ToTimeOfDayExclusive")
                        .HasColumnType("time");

                    b.Property<int>("VehicleTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("ChargeMatrix");

                    b.HasCheckConstraint("CK_ChargeMatrix_FromTimeOfDayInclusive_ToTimeOfDayExclusive", "(ToTimeOfDayExclusive > FromTimeOfDayInclusive)");

                    b.HasCheckConstraint("CK_ChargeMatrix_ChargePerEntry", "(ChargePerEntry >= 0)");
                });

            modelBuilder.Entity("Decongestor.Domain.HolidayConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<int?>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("HolidayConfiguration");

                    b.HasCheckConstraint("CK_HolidayConfiguration_Day", "(Day between 1 and 31)");

                    b.HasCheckConstraint("CK_HolidayConfiguration_Month", "(Month between 1 and 12)");

                    b.HasCheckConstraint("CK_HolidayConfiguration_Year", "(Year is null or Year between 2020 and 9999)");
                });

            modelBuilder.Entity("Decongestor.Domain.TollEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Charge")
                        .HasColumnType("decimal(5,2)");

                    b.Property<DateTime>("ChargeDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasComputedColumnSql("cast(EnteredAtUtc as date)");

                    b.Property<DateTime>("EnteredAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VehicleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("EnteredAtUtc");

                    b.HasIndex("VehicleId");

                    b.ToTable("TollEntries");

                    b.HasCheckConstraint("CK_TollEntry_Charge", "(Charge >= 0)");
                });

            modelBuilder.Entity("Decongestor.Domain.Vehicle", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("VehicleTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Decongestor.Domain.VehicleType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal?>("DailyChargeCap")
                        .HasColumnType("decimal(5,2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.ToTable("VehicleTypes");

                    b.HasCheckConstraint("CK_VehicleType_DailyChargeCap", "(DailyChargeCap is null or DailyChargeCap >= 0)");
                });

            modelBuilder.Entity("Decongestor.Domain.ChargeMatrix", b =>
                {
                    b.HasOne("Decongestor.Domain.VehicleType", "VehicleType")
                        .WithMany()
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("Decongestor.Domain.TollEntry", b =>
                {
                    b.HasOne("Decongestor.Domain.Vehicle", "Vehicle")
                        .WithMany("TollEntries")
                        .HasForeignKey("VehicleId");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Decongestor.Domain.Vehicle", b =>
                {
                    b.HasOne("Decongestor.Domain.VehicleType", "VehicleType")
                        .WithMany("Vehicles")
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("Decongestor.Domain.Vehicle", b =>
                {
                    b.Navigation("TollEntries");
                });

            modelBuilder.Entity("Decongestor.Domain.VehicleType", b =>
                {
                    b.Navigation("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}
