using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrendEmber.Data.Migrations.TrendsDb
{
    /// <inheritdoc />
    public partial class AddPeaksAndTroughsPriceRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PriceHistoryId",
                table: "WavePoints",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WavePoints_PriceHistoryId",
                table: "WavePoints",
                column: "PriceHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_WavePoints_EquityPrices_PriceHistoryId",
                table: "WavePoints",
                column: "PriceHistoryId",
                principalTable: "EquityPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WavePoints_EquityPrices_PriceHistoryId",
                table: "WavePoints");

            migrationBuilder.DropIndex(
                name: "IX_WavePoints_PriceHistoryId",
                table: "WavePoints");

            migrationBuilder.DropColumn(
                name: "PriceHistoryId",
                table: "WavePoints");
        }
    }
}
