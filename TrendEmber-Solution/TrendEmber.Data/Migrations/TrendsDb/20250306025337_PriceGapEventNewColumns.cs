using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrendEmber.Data.Migrations.TrendsDb
{
    /// <inheritdoc />
    public partial class PriceGapEventNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Close",
                table: "PriceGapEvents",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Open",
                table: "PriceGapEvents",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "PriceGapEvents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Close",
                table: "PriceGapEvents");

            migrationBuilder.DropColumn(
                name: "Open",
                table: "PriceGapEvents");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "PriceGapEvents");
        }
    }
}
