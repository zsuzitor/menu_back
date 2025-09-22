using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class task_reviewer_creator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatorEntityId",
                table: "ReviewTasks",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorEntityId",
                table: "ReviewTasks");
        }
    }
}
