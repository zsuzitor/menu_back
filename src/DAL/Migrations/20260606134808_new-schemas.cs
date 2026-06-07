using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class newschemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MenuApp");

            migrationBuilder.EnsureSchema(
                name: "PlaningPoker");

            migrationBuilder.EnsureSchema(
                name: "VaultApp");

            migrationBuilder.EnsureSchema(
                name: "TaskManagementApp");

            migrationBuilder.EnsureSchema(
                name: "WordsCards");

            migrationBuilder.RenameTable(
                name: "WordsLists",
                newName: "WordsLists",
                newSchema: "WordsCards");

            migrationBuilder.RenameTable(
                name: "WordsCards",
                newName: "WordsCards",
                newSchema: "WordsCards");

            migrationBuilder.RenameTable(
                name: "VaultUsers",
                newName: "VaultUsers",
                newSchema: "VaultApp");

            migrationBuilder.RenameTable(
                name: "Vaults",
                newName: "Vaults",
                newSchema: "VaultApp");

            migrationBuilder.RenameTable(
                name: "TaskManagementWorkTimeLog",
                newName: "TaskManagementWorkTimeLog",
                newSchema: "TaskManagementApp");

            migrationBuilder.RenameTable(
                name: "TaskManagementTaskStatus",
                newName: "TaskManagementTaskStatus",
                newSchema: "TaskManagementApp");

            migrationBuilder.RenameTable(
                name: "TaskManagementTasks",
                newName: "TaskManagementTasks",
                newSchema: "TaskManagementApp");

            migrationBuilder.RenameTable(
                name: "TaskManagementTaskRelation",
                newName: "TaskManagementTaskRelation",
                newSchema: "TaskManagementApp");

            migrationBuilder.RenameTable(
                name: "TaskManagementTaskComment",
                newName: "TaskManagementTaskComment",
                newSchema: "TaskManagementApp");

            migrationBuilder.RenameTable(
                name: "TaskManagementSprintTaskRelation",
                newName: "TaskManagementSprintTaskRelation",
                newSchema: "TaskManagementApp");

            migrationBuilder.RenameTable(
                name: "TaskManagementSprint",
                newName: "TaskManagementSprint",
                newSchema: "TaskManagementApp");

            migrationBuilder.RenameTable(
                name: "TaskManagementProjectUsers",
                newName: "TaskManagementProjectUsers",
                newSchema: "TaskManagementApp");

            migrationBuilder.RenameTable(
                name: "TaskManagementProjects",
                newName: "TaskManagementProjects",
                newSchema: "TaskManagementApp");

            migrationBuilder.RenameTable(
                name: "TaskManagementPreset",
                newName: "TaskManagementPreset",
                newSchema: "TaskManagementApp");

            migrationBuilder.RenameTable(
                name: "TaskManagementLabelTaskRelation",
                newName: "TaskManagementLabelTaskRelation",
                newSchema: "TaskManagementApp");

            migrationBuilder.RenameTable(
                name: "TaskManagementLabel",
                newName: "TaskManagementLabel",
                newSchema: "TaskManagementApp");

            migrationBuilder.RenameTable(
                name: "Secrets",
                newName: "Secrets",
                newSchema: "VaultApp");

            migrationBuilder.RenameTable(
                name: "PlaningStories",
                newName: "PlaningStories",
                newSchema: "PlaningPoker");

            migrationBuilder.RenameTable(
                name: "PlaningRoomUsers",
                newName: "PlaningRoomUsers",
                newSchema: "PlaningPoker");

            migrationBuilder.RenameTable(
                name: "PlaningRooms",
                newName: "PlaningRooms",
                newSchema: "PlaningPoker");

            migrationBuilder.RenameTable(
                name: "Articles",
                newName: "Articles",
                newSchema: "MenuApp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "WordsLists",
                schema: "WordsCards",
                newName: "WordsLists");

            migrationBuilder.RenameTable(
                name: "WordsCards",
                schema: "WordsCards",
                newName: "WordsCards");

            migrationBuilder.RenameTable(
                name: "VaultUsers",
                schema: "VaultApp",
                newName: "VaultUsers");

            migrationBuilder.RenameTable(
                name: "Vaults",
                schema: "VaultApp",
                newName: "Vaults");

            migrationBuilder.RenameTable(
                name: "TaskManagementWorkTimeLog",
                schema: "TaskManagementApp",
                newName: "TaskManagementWorkTimeLog");

            migrationBuilder.RenameTable(
                name: "TaskManagementTaskStatus",
                schema: "TaskManagementApp",
                newName: "TaskManagementTaskStatus");

            migrationBuilder.RenameTable(
                name: "TaskManagementTasks",
                schema: "TaskManagementApp",
                newName: "TaskManagementTasks");

            migrationBuilder.RenameTable(
                name: "TaskManagementTaskRelation",
                schema: "TaskManagementApp",
                newName: "TaskManagementTaskRelation");

            migrationBuilder.RenameTable(
                name: "TaskManagementTaskComment",
                schema: "TaskManagementApp",
                newName: "TaskManagementTaskComment");

            migrationBuilder.RenameTable(
                name: "TaskManagementSprintTaskRelation",
                schema: "TaskManagementApp",
                newName: "TaskManagementSprintTaskRelation");

            migrationBuilder.RenameTable(
                name: "TaskManagementSprint",
                schema: "TaskManagementApp",
                newName: "TaskManagementSprint");

            migrationBuilder.RenameTable(
                name: "TaskManagementProjectUsers",
                schema: "TaskManagementApp",
                newName: "TaskManagementProjectUsers");

            migrationBuilder.RenameTable(
                name: "TaskManagementProjects",
                schema: "TaskManagementApp",
                newName: "TaskManagementProjects");

            migrationBuilder.RenameTable(
                name: "TaskManagementPreset",
                schema: "TaskManagementApp",
                newName: "TaskManagementPreset");

            migrationBuilder.RenameTable(
                name: "TaskManagementLabelTaskRelation",
                schema: "TaskManagementApp",
                newName: "TaskManagementLabelTaskRelation");

            migrationBuilder.RenameTable(
                name: "TaskManagementLabel",
                schema: "TaskManagementApp",
                newName: "TaskManagementLabel");

            migrationBuilder.RenameTable(
                name: "Secrets",
                schema: "VaultApp",
                newName: "Secrets");

            migrationBuilder.RenameTable(
                name: "PlaningStories",
                schema: "PlaningPoker",
                newName: "PlaningStories");

            migrationBuilder.RenameTable(
                name: "PlaningRoomUsers",
                schema: "PlaningPoker",
                newName: "PlaningRoomUsers");

            migrationBuilder.RenameTable(
                name: "PlaningRooms",
                schema: "PlaningPoker",
                newName: "PlaningRooms");

            migrationBuilder.RenameTable(
                name: "Articles",
                schema: "MenuApp",
                newName: "Articles");
        }
    }
}
