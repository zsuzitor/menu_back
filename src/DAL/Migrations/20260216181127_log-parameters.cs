using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class logparameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Parameters",
                table: "MainLogTable",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Parameters",
                table: "MainLogTable");
        }
    }
}
