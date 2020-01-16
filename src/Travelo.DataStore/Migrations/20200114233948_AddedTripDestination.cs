using Microsoft.EntityFrameworkCore.Migrations;

namespace Travelo.DataStore.Migrations
{
    public partial class AddedTripDestination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TripDestination",
                table: "Trips",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TripDestination",
                table: "Trips");
        }
    }
}
