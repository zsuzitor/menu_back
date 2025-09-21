using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class sprintsmore_fix6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskManagementTasks_TaskManagementSprint_SprintId",
                table: "TaskManagementTasks");

            migrationBuilder.DropIndex(
                name: "IX_TaskManagementTasks_SprintId",
                table: "TaskManagementTasks");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "TaskManagementTaskStatus");

            migrationBuilder.DropColumn(
                name: "SprintId",
                table: "TaskManagementTasks");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "TaskManagementTaskComment",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "TaskManagementSprint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "TaskManagementSprint",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "TaskManagementLabel",
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
                    table.PrimaryKey("PK_TaskManagementLabel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskManagementSprintTaskRelation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<long>(nullable: false),
                    SprintId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskManagementSprintTaskRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskManagementSprintTaskRelation_TaskManagementSprint_SprintId",
                        column: x => x.SprintId,
                        principalTable: "TaskManagementSprint",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskManagementSprintTaskRelation_TaskManagementTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskManagementTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskManagementLabelTaskRelation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<long>(nullable: false),
                    LabelId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskManagementLabelTaskRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskManagementLabelTaskRelation_TaskManagementLabel_LabelId",
                        column: x => x.LabelId,
                        principalTable: "TaskManagementLabel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskManagementLabelTaskRelation_TaskManagementTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskManagementTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementLabelTaskRelation_LabelId",
                table: "TaskManagementLabelTaskRelation",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementLabelTaskRelation_TaskId",
                table: "TaskManagementLabelTaskRelation",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementSprintTaskRelation_SprintId",
                table: "TaskManagementSprintTaskRelation",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementSprintTaskRelation_TaskId",
                table: "TaskManagementSprintTaskRelation",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskManagementLabelTaskRelation");

            migrationBuilder.DropTable(
                name: "TaskManagementSprintTaskRelation");

            migrationBuilder.DropTable(
                name: "TaskManagementLabel");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "TaskManagementTaskComment");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "TaskManagementSprint");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "TaskManagementSprint");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "TaskManagementTaskStatus",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SprintId",
                table: "TaskManagementTasks",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementTasks_SprintId",
                table: "TaskManagementTasks",
                column: "SprintId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskManagementTasks_TaskManagementSprint_SprintId",
                table: "TaskManagementTasks",
                column: "SprintId",
                principalTable: "TaskManagementSprint",
                principalColumn: "Id");
        }
    }
}
