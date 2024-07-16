using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDealership.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _schemes_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Deals",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentId",
                table: "AutoConfigurations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AutoConfigurations_EquipmentId",
                table: "AutoConfigurations",
                column: "EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutoConfigurations_Equipments_EquipmentId",
                table: "AutoConfigurations",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutoConfigurations_Equipments_EquipmentId",
                table: "AutoConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_AutoConfigurations_EquipmentId",
                table: "AutoConfigurations");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "EquipmentId",
                table: "AutoConfigurations");
        }
    }
}
