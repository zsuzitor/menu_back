using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class configexpire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Configurations_Key",
                table: "Configurations");

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "Configurations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Start",
                table: "Configurations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Configurations_Key",
                table: "Configurations",
                column: "Key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Configurations_Key",
                table: "Configurations");

            migrationBuilder.DropColumn(
                name: "End",
                table: "Configurations");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "Configurations");

            migrationBuilder.CreateIndex(
                name: "IX_Configurations_Key",
                table: "Configurations",
                column: "Key",
                unique: true,
                filter: "[Key] IS NOT NULL");
        }
    }
}
