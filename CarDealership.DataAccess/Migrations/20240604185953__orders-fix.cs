using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDealership.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _ordersfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cars_CarId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "Orders",
                newName: "AutoConfigurationId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CarId",
                table: "Orders",
                newName: "IX_Orders_AutoConfigurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AutoConfigurations_AutoConfigurationId",
                table: "Orders",
                column: "AutoConfigurationId",
                principalTable: "AutoConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AutoConfigurations_AutoConfigurationId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "AutoConfigurationId",
                table: "Orders",
                newName: "CarId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_AutoConfigurationId",
                table: "Orders",
                newName: "IX_Orders_CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cars_CarId",
                table: "Orders",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
