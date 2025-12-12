using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class preset1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskManagementPreset",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    CreatorId = table.Column<long>(nullable: true),
                    ExecutorId = table.Column<long>(nullable: true),
                    StatusId = table.Column<long>(nullable: true),
                    SprintId = table.Column<long>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskManagementPreset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskManagementPreset_TaskManagementProjectUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "TaskManagementProjectUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskManagementPreset_TaskManagementProjectUsers_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "TaskManagementProjectUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskManagementPreset_TaskManagementProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "TaskManagementProjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskManagementPreset_TaskManagementSprintTaskRelation_SprintId",
                        column: x => x.SprintId,
                        principalTable: "TaskManagementSprintTaskRelation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskManagementPreset_TaskManagementTaskStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "TaskManagementTaskStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskManagementPresetRelation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PresetId = table.Column<long>(nullable: false),
                    LabelId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskManagementPresetRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskManagementPresetRelation_TaskManagementLabel_LabelId",
                        column: x => x.LabelId,
                        principalTable: "TaskManagementLabel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskManagementPresetRelation_TaskManagementPreset_PresetId",
                        column: x => x.PresetId,
                        principalTable: "TaskManagementPreset",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementPreset_CreatorId",
                table: "TaskManagementPreset",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementPreset_ExecutorId",
                table: "TaskManagementPreset",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementPreset_ProjectId",
                table: "TaskManagementPreset",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementPreset_SprintId",
                table: "TaskManagementPreset",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementPreset_StatusId",
                table: "TaskManagementPreset",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementPresetRelation_LabelId",
                table: "TaskManagementPresetRelation",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementPresetRelation_PresetId",
                table: "TaskManagementPresetRelation",
                column: "PresetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskManagementPresetRelation");

            migrationBuilder.DropTable(
                name: "TaskManagementPreset");
        }
    }
}
