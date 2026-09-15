using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class eventcountrename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CountChage",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "CountChange");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CountChange",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "CountChage");
        }
    }
}
