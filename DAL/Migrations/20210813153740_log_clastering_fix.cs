using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class log_clastering_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MainLogTable",
                table: "MainLogTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MainLogTable",
                table: "MainLogTable",
                column: "Id")
                .Annotation("SqlServer:Clustered", false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MainLogTable",
                table: "MainLogTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MainLogTable",
                table: "MainLogTable",
                column: "Id");
        }
    }
}
