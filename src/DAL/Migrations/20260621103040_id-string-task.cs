using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class idstringtask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdString",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks",
                type: "nvarchar(450)",
                nullable: true,
                computedColumnSql: "CAST([Id] AS NVARCHAR(20))",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementTasks_IdString",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks",
                column: "IdString");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskManagementTasks_IdString",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks");

            migrationBuilder.DropColumn(
                name: "IdString",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks");
        }
    }
}
