using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapie.WebUI.Migrations
{
    public partial class membershipComplated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Membership",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Surname",
                schema: "Membership",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                schema: "Membership",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
