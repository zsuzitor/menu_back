using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class logid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
            name: "PK_MainLogTable",
            table: "MainLogTable");

            // 3. Удаляем старую колонку
            migrationBuilder.DropColumn(
                name: "Id",
                table: "MainLogTable");

            // 4. Создаем новую колонку с Identity
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "MainLogTable",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MainLogTable",
                table: "MainLogTable",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Если нужно вернуться к GUID
            migrationBuilder.DropPrimaryKey(
                name: "PK_MainLogTable",
                table: "MainLogTable");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MainLogTable",
                nullable: false,
                oldClrType: typeof(long),
                oldDefaultValue: 0L)
                .Annotation("SqlServer:DefaultValueSql", "NEWID()")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MainLogTable",
                table: "MainLogTable",
                column: "Id");
        }
    }
}
