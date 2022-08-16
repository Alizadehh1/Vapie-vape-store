using Microsoft.EntityFrameworkCore.Migrations;

namespace Vapie.WebUI.Migrations
{
    public partial class usernae : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductComments");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ProductComments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ProductComments");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ProductComments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
