using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrendEmber.Data.Migrations.TrendsDb
{
    /// <inheritdoc />
    public partial class AddEquityPriceHistoryShapeScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MeanRange",
                table: "Symbols",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StandardDeviation",
                table: "Symbols",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RangeZScore",
                table: "EquityPrices",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Shape",
                table: "EquityPrices",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeanRange",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "StandardDeviation",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "RangeZScore",
                table: "EquityPrices");

            migrationBuilder.DropColumn(
                name: "Shape",
                table: "EquityPrices");
        }
    }
}
