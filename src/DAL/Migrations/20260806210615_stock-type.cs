using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class stocktype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "FinancialAssistantApp");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Portfolio",
                schema: "FinancialAssistantApp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portfolio_TaskManagementProjectUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "TaskManagementApp",
                        principalTable: "TaskManagementProjectUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                schema: "FinancialAssistantApp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ActualizationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    IsGlobal = table.Column<bool>(type: "bit", nullable: false),
                    PortfolioId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stock_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalSchema: "FinancialAssistantApp",
                        principalTable: "Portfolio",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stock_Stock_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "FinancialAssistantApp",
                        principalTable: "Stock",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockElement",
                schema: "FinancialAssistantApp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<long>(type: "bigint", nullable: false),
                    Count = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PortfolioId = table.Column<long>(type: "bigint", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockElement_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalSchema: "FinancialAssistantApp",
                        principalTable: "Portfolio",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockElement_Stock_StockId",
                        column: x => x.StockId,
                        principalSchema: "FinancialAssistantApp",
                        principalTable: "Stock",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockHistory",
                schema: "FinancialAssistantApp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockId = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockHistory_Stock_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "FinancialAssistantApp",
                        principalTable: "Stock",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockHistory_Stock_StockId",
                        column: x => x.StockId,
                        principalSchema: "FinancialAssistantApp",
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockEvent",
                schema: "FinancialAssistantApp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Count = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    StockElementId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    PortfolioId = table.Column<long>(type: "bigint", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockEvent_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalSchema: "FinancialAssistantApp",
                        principalTable: "Portfolio",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockEvent_StockElement_StockElementId",
                        column: x => x.StockElementId,
                        principalSchema: "FinancialAssistantApp",
                        principalTable: "StockElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockEvent_Stock_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "FinancialAssistantApp",
                        principalTable: "Stock",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Portfolio_CurrencyId",
                schema: "FinancialAssistantApp",
                table: "Portfolio",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolio_UserId",
                schema: "FinancialAssistantApp",
                table: "Portfolio",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_Code",
                schema: "FinancialAssistantApp",
                table: "Stock",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_CurrencyId",
                schema: "FinancialAssistantApp",
                table: "Stock",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_PortfolioId",
                schema: "FinancialAssistantApp",
                table: "Stock",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_StockElement_PortfolioId",
                schema: "FinancialAssistantApp",
                table: "StockElement",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_StockElement_StockId",
                schema: "FinancialAssistantApp",
                table: "StockElement",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockEvent_CurrencyId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_StockEvent_PortfolioId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_StockEvent_StockElementId",
                schema: "FinancialAssistantApp",
                table: "StockEvent",
                column: "StockElementId");

            migrationBuilder.CreateIndex(
                name: "IX_StockHistory_CurrencyId",
                schema: "FinancialAssistantApp",
                table: "StockHistory",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_StockHistory_StockId",
                schema: "FinancialAssistantApp",
                table: "StockHistory",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolio_Stock_CurrencyId",
                schema: "FinancialAssistantApp",
                table: "Portfolio",
                column: "CurrencyId",
                principalSchema: "FinancialAssistantApp",
                principalTable: "Stock",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portfolio_Stock_CurrencyId",
                schema: "FinancialAssistantApp",
                table: "Portfolio");

            migrationBuilder.DropTable(
                name: "StockEvent",
                schema: "FinancialAssistantApp");

            migrationBuilder.DropTable(
                name: "StockHistory",
                schema: "FinancialAssistantApp");

            migrationBuilder.DropTable(
                name: "StockElement",
                schema: "FinancialAssistantApp");

            migrationBuilder.DropTable(
                name: "Stock",
                schema: "FinancialAssistantApp");

            migrationBuilder.DropTable(
                name: "Portfolio",
                schema: "FinancialAssistantApp");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");
        }
    }
}
