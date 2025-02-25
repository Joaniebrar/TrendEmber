using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrendEmber.Data.Migrations.TrendsDb
{
    /// <inheritdoc />
    public partial class ApiProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HarvesterAgentId",
                table: "WatchList",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApiProvider",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BaseUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiProvider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HarvesterAgent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApiProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HarvesterAgent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HarvesterAgent_ApiProvider_ApiProviderId",
                        column: x => x.ApiProviderId,
                        principalTable: "ApiProvider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchList_HarvesterAgentId",
                table: "WatchList",
                column: "HarvesterAgentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HarvesterAgent_ApiProviderId",
                table: "HarvesterAgent",
                column: "ApiProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchList_HarvesterAgent_HarvesterAgentId",
                table: "WatchList",
                column: "HarvesterAgentId",
                principalTable: "HarvesterAgent",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchList_HarvesterAgent_HarvesterAgentId",
                table: "WatchList");

            migrationBuilder.DropTable(
                name: "HarvesterAgent");

            migrationBuilder.DropTable(
                name: "ApiProvider");

            migrationBuilder.DropIndex(
                name: "IX_WatchList_HarvesterAgentId",
                table: "WatchList");

            migrationBuilder.DropColumn(
                name: "HarvesterAgentId",
                table: "WatchList");
        }
    }
}
