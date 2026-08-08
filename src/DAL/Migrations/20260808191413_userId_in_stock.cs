using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class userId_in_stock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stock_Portfolio_PortfolioId",
                schema: "FinancialAssistantApp",
                table: "Stock");

            migrationBuilder.RenameColumn(
                name: "PortfolioId",
                schema: "FinancialAssistantApp",
                table: "Stock",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Stock_PortfolioId",
                schema: "FinancialAssistantApp",
                table: "Stock",
                newName: "IX_Stock_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_Users_UserId",
                schema: "FinancialAssistantApp",
                table: "Stock",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stock_Users_UserId",
                schema: "FinancialAssistantApp",
                table: "Stock");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "FinancialAssistantApp",
                table: "Stock",
                newName: "PortfolioId");

            migrationBuilder.RenameIndex(
                name: "IX_Stock_UserId",
                schema: "FinancialAssistantApp",
                table: "Stock",
                newName: "IX_Stock_PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_Portfolio_PortfolioId",
                schema: "FinancialAssistantApp",
                table: "Stock",
                column: "PortfolioId",
                principalSchema: "FinancialAssistantApp",
                principalTable: "Portfolio",
                principalColumn: "Id");
        }
    }
}
