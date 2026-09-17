using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class event_elem1_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockEvent_StockElement_StockElementId",
                schema: "FinancialAssistantApp",
                table: "StockEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_StockEvent_Stock_CurrencyId",
                schema: "FinancialAssistantApp",
                table: "StockEvent");

            migrationBuilder.RenameColumn(
                name: "StockElementId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "MainElementId");

            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "SubCountNow");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "SubElementId");

            migrationBuilder.RenameColumn(
                name: "CountNow",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "MainCountNow");

            migrationBuilder.RenameColumn(
                name: "CountChange",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "MainCountChange");

            migrationBuilder.RenameIndex(
                name: "IX_StockEvent_StockElementId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "IX_StockEvent_MainElementId");

            migrationBuilder.RenameIndex(
                name: "IX_StockEvent_CurrencyId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "IX_StockEvent_SubElementId");

            migrationBuilder.AddColumn<decimal>(
                name: "SubCountChange",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StockEvent_StockElement_MainElementId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                column: "MainElementId",
                principalSchema: "FinancialAssistantApp",
                principalTable: "StockElement",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockEvent_StockElement_SubElementId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                column: "SubElementId",
                principalSchema: "FinancialAssistantApp",
                principalTable: "StockElement",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockEvent_StockElement_MainElementId",
                schema: "FinancialAssistantApp",
                table: "StockEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_StockEvent_StockElement_SubElementId",
                schema: "FinancialAssistantApp",
                table: "StockEvent");

            migrationBuilder.DropColumn(
                name: "SubCountChange",
                schema: "FinancialAssistantApp",
                table: "StockEvent");

            migrationBuilder.RenameColumn(
                name: "SubElementId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "CurrencyId");

            migrationBuilder.RenameColumn(
                name: "SubCountNow",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "MainElementId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "StockElementId");

            migrationBuilder.RenameColumn(
                name: "MainCountNow",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "CountNow");

            migrationBuilder.RenameColumn(
                name: "MainCountChange",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "CountChange");

            migrationBuilder.RenameIndex(
                name: "IX_StockEvent_SubElementId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "IX_StockEvent_CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_StockEvent_MainElementId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "IX_StockEvent_StockElementId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockEvent_StockElement_StockElementId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                column: "StockElementId",
                principalSchema: "FinancialAssistantApp",
                principalTable: "StockElement",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockEvent_Stock_CurrencyId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                column: "CurrencyId",
                principalSchema: "FinancialAssistantApp",
                principalTable: "Stock",
                principalColumn: "Id");
        }
    }
}
