using Microsoft.EntityFrameworkCore.Migrations;

namespace Travelo.DataStore.Migrations
{
    public partial class AddedNamesAndDescriptionsToModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TripStatus",
                table: "Trips",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Trips",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Trips",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Customers",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Trips_Name",
                table: "Trips",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Customers_FullName",
                table: "Customers",
                column: "FullName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Trips_Name",
                table: "Trips");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Customers_FullName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Customers");

            migrationBuilder.AlterColumn<string>(
                name: "TripStatus",
                table: "Trips",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);
        }
    }
}
