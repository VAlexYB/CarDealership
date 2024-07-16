using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDealership.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _add_deletion_date : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "TransmissionTypes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Features",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Equipments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "EquipmentFeatures",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "EngineTypes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Engines",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "DriveTypes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Deals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Countries",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Colors",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Cars",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Brands",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "BodyTypes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "AutoModels",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "AutoConfigurations",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "TransmissionTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "EquipmentFeatures");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "EngineTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DriveTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "BodyTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "AutoModels");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "AutoConfigurations");
        }
    }
}
