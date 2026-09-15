using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class eventcounttotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Count",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "CountNow");

            migrationBuilder.AddColumn<decimal>(
                name: "CountChage",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountChage",
                schema: "FinancialAssistantApp",
                table: "StockEvent");

            migrationBuilder.RenameColumn(
                name: "CountNow",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "Count");
        }
    }
}
