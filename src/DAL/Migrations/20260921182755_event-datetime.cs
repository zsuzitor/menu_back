using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class eventdatetime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "EventDateTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateTime",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDateTime",
                schema: "FinancialAssistantApp",
                table: "StockEvent");

            migrationBuilder.RenameColumn(
                name: "EventDateTime",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                newName: "Date");
        }
    }
}
