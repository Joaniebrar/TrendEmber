using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrendEmber.Data.Migrations.TrendsDb
{
    /// <inheritdoc />
    public partial class TradeSetupSimulatorMissingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Entry",
                table: "TradeSetupSimulations",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "TradeSetupSimulations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TradeDate",
                table: "TradeSetupSimulations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entry",
                table: "TradeSetupSimulations");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "TradeSetupSimulations");

            migrationBuilder.DropColumn(
                name: "TradeDate",
                table: "TradeSetupSimulations");
        }
    }
}
