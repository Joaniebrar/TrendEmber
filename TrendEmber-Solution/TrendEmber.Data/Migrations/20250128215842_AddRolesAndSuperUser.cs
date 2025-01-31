using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrendEmber.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesAndSuperUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Roles
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName" },
                values: new object[,]
                {
                { Guid.NewGuid().ToString(), "Admin", "ADMIN" },
                { Guid.NewGuid().ToString(), "Analyst", "ANALYST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
