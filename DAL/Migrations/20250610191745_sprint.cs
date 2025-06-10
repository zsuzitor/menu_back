using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class sprint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SprintId",
                table: "TaskManagementTasks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskManagementSprint",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskManagementSprint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskManagementSprint_TaskManagementProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "TaskManagementProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementSprint_ProjectId",
                table: "TaskManagementSprint",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskManagementTasks_TaskManagementSprint_StatusId",
                table: "TaskManagementTasks",
                column: "StatusId",
                principalTable: "TaskManagementSprint",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskManagementTasks_TaskManagementSprint_StatusId",
                table: "TaskManagementTasks");

            migrationBuilder.DropTable(
                name: "TaskManagementSprint");

            migrationBuilder.DropColumn(
                name: "SprintId",
                table: "TaskManagementTasks");
        }
    }
}
