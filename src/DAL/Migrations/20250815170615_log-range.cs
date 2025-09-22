using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class logrange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RangeEndOfLog",
                table: "TaskManagementWorkTimeLog",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RangeStartOfLog",
                table: "TaskManagementWorkTimeLog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RangeEndOfLog",
                table: "TaskManagementWorkTimeLog");

            migrationBuilder.DropColumn(
                name: "RangeStartOfLog",
                table: "TaskManagementWorkTimeLog");
        }
    }
}
