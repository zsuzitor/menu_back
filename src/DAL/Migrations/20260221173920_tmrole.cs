using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class tmrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deactivated",
                table: "TaskManagementProjectUsers");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "TaskManagementProjectUsers");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "TaskManagementProjectUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "TaskManagementProjectUsers");

            migrationBuilder.AddColumn<bool>(
                name: "Deactivated",
                table: "TaskManagementProjectUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "TaskManagementProjectUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
