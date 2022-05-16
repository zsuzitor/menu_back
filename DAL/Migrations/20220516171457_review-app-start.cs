using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class reviewappstart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewProject",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewProjectUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAdmin = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Deactivated = table.Column<bool>(nullable: false),
                    NotifyEmail = table.Column<string>(nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    MainAppUserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewProjectUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewProjectUsers_Users_MainAppUserId",
                        column: x => x.MainAppUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReviewProjectUsers_ReviewProject_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ReviewProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewTasks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false),
                    ReviewerId = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewTasks_ReviewProjectUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "ReviewProjectUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReviewTasks_ReviewProject_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ReviewProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewTasks_ReviewProjectUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "ReviewProjectUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReviewComment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    TaskId = table.Column<long>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewComment_ReviewProjectUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "ReviewProjectUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReviewComment_ReviewTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "ReviewTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComment_CreatorId",
                table: "ReviewComment",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComment_TaskId",
                table: "ReviewComment",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewProjectUsers_MainAppUserId",
                table: "ReviewProjectUsers",
                column: "MainAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewProjectUsers_ProjectId",
                table: "ReviewProjectUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewTasks_CreatorId",
                table: "ReviewTasks",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewTasks_ProjectId",
                table: "ReviewTasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewTasks_ReviewerId",
                table: "ReviewTasks",
                column: "ReviewerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewComment");

            migrationBuilder.DropTable(
                name: "ReviewTasks");

            migrationBuilder.DropTable(
                name: "ReviewProjectUsers");

            migrationBuilder.DropTable(
                name: "ReviewProject");
        }
    }
}
