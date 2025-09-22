using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class logtableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "MainLogTable",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActionDateEnd",
                table: "MainLogTable",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActionDateStart",
                table: "MainLogTable",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "MainLogTable",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "MainLogTable",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MainLogTable",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "MainLogTable");

            migrationBuilder.DropColumn(
                name: "ActionDateEnd",
                table: "MainLogTable");

            migrationBuilder.DropColumn(
                name: "ActionDateStart",
                table: "MainLogTable");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "MainLogTable");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "MainLogTable");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MainLogTable");
        }
    }
}
