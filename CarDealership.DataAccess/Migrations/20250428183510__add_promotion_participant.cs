using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDealership.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _add_promotion_participant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PromotionEntityUserEntity",
                columns: table => new
                {
                    ParticipantsId = table.Column<Guid>(type: "uuid", nullable: false),
                    PromotionEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionEntityUserEntity", x => new { x.ParticipantsId, x.PromotionEntityId });
                    table.ForeignKey(
                        name: "FK_PromotionEntityUserEntity_Promotions_PromotionEntityId",
                        column: x => x.PromotionEntityId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionEntityUserEntity_Users_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionEntityUserEntity_PromotionEntityId",
                table: "PromotionEntityUserEntity",
                column: "PromotionEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionEntityUserEntity");
        }
    }
}
