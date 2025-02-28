using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrendEmber.Data.Migrations.TrendsDb
{
    /// <inheritdoc />
    public partial class AddHarvesterRunFunctionality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HarvesterAgent_ApiProvider_ApiProviderId",
                table: "HarvesterAgent");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchList_HarvesterAgent_HarvesterAgentId",
                table: "WatchList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HarvesterAgent",
                table: "HarvesterAgent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiProvider",
                table: "ApiProvider");

            migrationBuilder.RenameTable(
                name: "HarvesterAgent",
                newName: "Agents");

            migrationBuilder.RenameTable(
                name: "ApiProvider",
                newName: "ApiProviders");

            migrationBuilder.RenameIndex(
                name: "IX_HarvesterAgent_ApiProviderId",
                table: "Agents",
                newName: "IX_Agents_ApiProviderId");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastImportedDate",
                table: "Symbols",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agents",
                table: "Agents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiProviders",
                table: "ApiProviders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Agents_ApiProviders_ApiProviderId",
                table: "Agents",
                column: "ApiProviderId",
                principalTable: "ApiProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchList_Agents_HarvesterAgentId",
                table: "WatchList",
                column: "HarvesterAgentId",
                principalTable: "Agents",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agents_ApiProviders_ApiProviderId",
                table: "Agents");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchList_Agents_HarvesterAgentId",
                table: "WatchList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiProviders",
                table: "ApiProviders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agents",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "LastImportedDate",
                table: "Symbols");

            migrationBuilder.RenameTable(
                name: "ApiProviders",
                newName: "ApiProvider");

            migrationBuilder.RenameTable(
                name: "Agents",
                newName: "HarvesterAgent");

            migrationBuilder.RenameIndex(
                name: "IX_Agents_ApiProviderId",
                table: "HarvesterAgent",
                newName: "IX_HarvesterAgent_ApiProviderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiProvider",
                table: "ApiProvider",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HarvesterAgent",
                table: "HarvesterAgent",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HarvesterAgent_ApiProvider_ApiProviderId",
                table: "HarvesterAgent",
                column: "ApiProviderId",
                principalTable: "ApiProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchList_HarvesterAgent_HarvesterAgentId",
                table: "WatchList",
                column: "HarvesterAgentId",
                principalTable: "HarvesterAgent",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
