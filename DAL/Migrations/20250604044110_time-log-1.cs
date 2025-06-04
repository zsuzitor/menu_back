using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class timelog1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskManagementWorkTimeLog",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(nullable: true),
                    TimeMinutes = table.Column<long>(nullable: false),
                    DayOfLog = table.Column<DateTime>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    WorkTaskId = table.Column<long>(nullable: false),
                    ProjectUserId = table.Column<long>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskManagementWorkTimeLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskManagementWorkTimeLog_TaskManagementProjectUsers_ProjectUserId",
                        column: x => x.ProjectUserId,
                        principalTable: "TaskManagementProjectUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskManagementWorkTimeLog_TaskManagementTasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "TaskManagementTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementWorkTimeLog_ProjectUserId",
                table: "TaskManagementWorkTimeLog",
                column: "ProjectUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementWorkTimeLog_WorkTaskId",
                table: "TaskManagementWorkTimeLog",
                column: "WorkTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskManagementWorkTimeLog");
        }
    }
}
