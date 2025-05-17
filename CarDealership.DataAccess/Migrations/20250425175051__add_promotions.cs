using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDealership.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _add_promotions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Promocode = table.Column<string>(type: "text", nullable: false),
                    OrderDiscountPercent = table.Column<decimal>(type: "numeric", nullable: true),
                    DealDiscountPercent = table.Column<decimal>(type: "numeric", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                    table.CheckConstraint("CK_Promotion_Dates", "\"EndDate\" > \"StartDate\"");
                    table.CheckConstraint("CK_Promotion_Discounts", "\"OrderDiscountPercent\" IS NOT NULL OR \"DealDiscountPercent\" IS NOT NULL");
                });

            migrationBuilder.CreateTable(
                name: "AutoConfigurationEntityPromotionEntity",
                columns: table => new
                {
                    AppliableConfigsId = table.Column<Guid>(type: "uuid", nullable: false),
                    PromotionEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoConfigurationEntityPromotionEntity", x => new { x.AppliableConfigsId, x.PromotionEntityId });
                    table.ForeignKey(
                        name: "FK_AutoConfigurationEntityPromotionEntity_AutoConfigurations_A~",
                        column: x => x.AppliableConfigsId,
                        principalTable: "AutoConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutoConfigurationEntityPromotionEntity_Promotions_Promotion~",
                        column: x => x.PromotionEntityId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutoConfigurationEntityPromotionEntity_PromotionEntityId",
                table: "AutoConfigurationEntityPromotionEntity",
                column: "PromotionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_Promocode",
                table: "Promotions",
                column: "Promocode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoConfigurationEntityPromotionEntity");

            migrationBuilder.DropTable(
                name: "Promotions");
        }
    }
}
