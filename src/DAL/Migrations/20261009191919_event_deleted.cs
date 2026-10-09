using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class event_deleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "FinancialAssistantApp",
                table: "Stock",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockHistory_Date",
                schema: "FinancialAssistantApp",
                table: "StockHistory",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_StockEvent_EventDateTime",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                column: "EventDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_Name",
                schema: "FinancialAssistantApp",
                table: "Stock",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockHistory_Date",
                schema: "FinancialAssistantApp",
                table: "StockHistory");

            migrationBuilder.DropIndex(
                name: "IX_StockEvent_EventDateTime",
                schema: "FinancialAssistantApp",
                table: "StockEvent");

            migrationBuilder.DropIndex(
                name: "IX_Stock_Name",
                schema: "FinancialAssistantApp",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "FinancialAssistantApp",
                table: "StockEvent");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "FinancialAssistantApp",
                table: "Stock",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
