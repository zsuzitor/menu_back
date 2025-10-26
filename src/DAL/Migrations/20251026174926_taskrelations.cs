using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class taskrelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskManagementTaskRelation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainWorkTaskId = table.Column<long>(nullable: false),
                    SubWorkTaskId = table.Column<long>(nullable: false),
                    RelationType = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskManagementTaskRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskManagementTaskRelation_TaskManagementTasks_MainWorkTaskId",
                        column: x => x.MainWorkTaskId,
                        principalTable: "TaskManagementTasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskManagementTaskRelation_TaskManagementTasks_SubWorkTaskId",
                        column: x => x.SubWorkTaskId,
                        principalTable: "TaskManagementTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementTaskRelation_MainWorkTaskId",
                table: "TaskManagementTaskRelation",
                column: "MainWorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementTaskRelation_SubWorkTaskId",
                table: "TaskManagementTaskRelation",
                column: "SubWorkTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskManagementTaskRelation");
        }
    }
}
