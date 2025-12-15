using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class presetlabelsfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskManagementPreset_TaskManagementSprintTaskRelation_SprintId",
                table: "TaskManagementPreset");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskManagementPreset_TaskManagementSprint_SprintId",
                table: "TaskManagementPreset",
                column: "SprintId",
                principalTable: "TaskManagementSprint",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskManagementPreset_TaskManagementSprint_SprintId",
                table: "TaskManagementPreset");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskManagementPreset_TaskManagementSprintTaskRelation_SprintId",
                table: "TaskManagementPreset",
                column: "SprintId",
                principalTable: "TaskManagementSprintTaskRelation",
                principalColumn: "Id");
        }
    }
}
