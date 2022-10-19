using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class poker_cards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "ReviewTasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cards",
                table: "PlaningRooms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "ReviewTasks");

            migrationBuilder.DropColumn(
                name: "Cards",
                table: "PlaningRooms");
        }
    }
}
