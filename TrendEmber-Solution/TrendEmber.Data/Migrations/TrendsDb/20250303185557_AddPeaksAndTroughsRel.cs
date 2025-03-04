using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrendEmber.Data.Migrations.TrendsDb
{
    /// <inheritdoc />
    public partial class AddPeaksAndTroughsRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SymbolId",
                table: "WavePoints",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WavePoints_SymbolId",
                table: "WavePoints",
                column: "SymbolId");

            migrationBuilder.AddForeignKey(
                name: "FK_WavePoints_Symbols_SymbolId",
                table: "WavePoints",
                column: "SymbolId",
                principalTable: "Symbols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WavePoints_Symbols_SymbolId",
                table: "WavePoints");

            migrationBuilder.DropIndex(
                name: "IX_WavePoints_SymbolId",
                table: "WavePoints");

            migrationBuilder.DropColumn(
                name: "SymbolId",
                table: "WavePoints");
        }
    }
}
