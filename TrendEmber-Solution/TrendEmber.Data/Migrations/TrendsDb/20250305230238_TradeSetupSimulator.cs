using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrendEmber.Data.Migrations.TrendsDb
{
    /// <inheritdoc />
    public partial class TradeSetupSimulator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradeSetupSimulations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TradeSetupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    FirstResistance = table.Column<decimal>(type: "numeric", nullable: true),
                    SecondResistance = table.Column<decimal>(type: "numeric", nullable: true),
                    FirstSupport = table.Column<decimal>(type: "numeric", nullable: true),
                    SecondSupport = table.Column<decimal>(type: "numeric", nullable: true),
                    TG1 = table.Column<decimal>(type: "numeric", nullable: true),
                    TG2 = table.Column<decimal>(type: "numeric", nullable: true),
                    SL = table.Column<decimal>(type: "numeric", nullable: true),
                    TG1Percentage = table.Column<decimal>(type: "numeric", nullable: true),
                    TG2Percentage = table.Column<decimal>(type: "numeric", nullable: true),
                    SLPercentage = table.Column<decimal>(type: "numeric", nullable: true),
                    Exit = table.Column<decimal>(type: "numeric", nullable: true),
                    ExitPercentage = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeSetupSimulations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeSetupSimulations_TradeSetups_TradeSetupId",
                        column: x => x.TradeSetupId,
                        principalTable: "TradeSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradeSetupSimulations_TradeSetupId",
                table: "TradeSetupSimulations",
                column: "TradeSetupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeSetupSimulations");
        }
    }
}
