using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class room_img : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "PlaningRooms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "PlaningRooms");
        }
    }
}
