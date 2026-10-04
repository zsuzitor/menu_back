using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class fix_portfolio_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portfolio_TaskManagementProjectUsers_UserId",
                schema: "FinancialAssistantApp",
                table: "Portfolio");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolio_Users_UserId",
                schema: "FinancialAssistantApp",
                table: "Portfolio",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portfolio_Users_UserId",
                schema: "FinancialAssistantApp",
                table: "Portfolio");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolio_TaskManagementProjectUsers_UserId",
                schema: "FinancialAssistantApp",
                table: "Portfolio",
                column: "UserId",
                principalSchema: "TaskManagementApp",
                principalTable: "TaskManagementProjectUsers",
                principalColumn: "Id");
        }
    }
}
