using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class tasktades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "ReviewTasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ReviewTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ReviewTasks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "ReviewTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ReviewTasks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ReviewTasks");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "ReviewTasks");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "ReviewTasks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
