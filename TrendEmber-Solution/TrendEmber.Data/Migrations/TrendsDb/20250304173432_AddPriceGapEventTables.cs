using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrendEmber.Data.Migrations.TrendsDb
{
    /// <inheritdoc />
    public partial class AddPriceGapEventTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceGapEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClosingEquityPriceHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    OpeningEquityPriceHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Direction = table.Column<int>(type: "integer", nullable: false),
                    GapFilledPriceHistoryId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceGapEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceGapEvents_EquityPrices_ClosingEquityPriceHistoryId",
                        column: x => x.ClosingEquityPriceHistoryId,
                        principalTable: "EquityPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceGapEvents_EquityPrices_GapFilledPriceHistoryId",
                        column: x => x.GapFilledPriceHistoryId,
                        principalTable: "EquityPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PriceGapEvents_EquityPrices_OpeningEquityPriceHistoryId",
                        column: x => x.OpeningEquityPriceHistoryId,
                        principalTable: "EquityPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceGapEvents_ClosingEquityPriceHistoryId",
                table: "PriceGapEvents",
                column: "ClosingEquityPriceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceGapEvents_GapFilledPriceHistoryId",
                table: "PriceGapEvents",
                column: "GapFilledPriceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceGapEvents_OpeningEquityPriceHistoryId",
                table: "PriceGapEvents",
                column: "OpeningEquityPriceHistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceGapEvents");
        }
    }
}
