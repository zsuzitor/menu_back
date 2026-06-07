using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class fixmigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TaskManagementPresetRelation",
                newName: "TaskManagementPresetRelation",
                newSchema: "TaskManagementApp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TaskManagementPresetRelation",
                schema: "TaskManagementApp",
                newName: "TaskManagementPresetRelation");
        }
    }
}
