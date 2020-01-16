using Microsoft.EntityFrameworkCore.Migrations;

namespace Travelo.DataStore.Migrations
{
    public partial class RemovedUniqunessOfNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Trips_Name",
                table: "Trips");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Customers_FullName",
                table: "Customers");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_Name",
                table: "Trips",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_FullName",
                table: "Customers",
                column: "FullName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trips_Name",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Customers_FullName",
                table: "Customers");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Trips_Name",
                table: "Trips",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Customers_FullName",
                table: "Customers",
                column: "FullName");
        }
    }
}
