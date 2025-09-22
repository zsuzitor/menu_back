using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class renamereview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewComment");

            migrationBuilder.DropTable(
                name: "ReviewTasks");

            migrationBuilder.DropTable(
                name: "ReviewProjectUsers");

            migrationBuilder.DropTable(
                name: "TaskReviewStatus");

            migrationBuilder.DropTable(
                name: "ReviewProject");

            migrationBuilder.CreateTable(
                name: "TaskManagementProjects",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskManagementProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskManagementProjectUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAdmin = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Deactivated = table.Column<bool>(nullable: false),
                    NotifyEmail = table.Column<string>(nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    MainAppUserId = table.Column<long>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskManagementProjectUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskManagementProjectUsers_Users_MainAppUserId",
                        column: x => x.MainAppUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskManagementProjectUsers_TaskManagementProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "TaskManagementProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskManagementTaskStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskManagementTaskStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskManagementTaskStatus_TaskManagementProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "TaskManagementProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskManagementTasks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CreatorEntityId = table.Column<long>(nullable: false),
                    StatusId = table.Column<long>(nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false),
                    ExecutorId = table.Column<long>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskManagementTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskManagementTasks_TaskManagementProjectUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "TaskManagementProjectUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskManagementTasks_TaskManagementProjectUsers_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "TaskManagementProjectUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskManagementTasks_TaskManagementProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "TaskManagementProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskManagementTasks_TaskManagementTaskStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "TaskManagementTaskStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskManagementTaskComment",
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
                    table.PrimaryKey("PK_TaskManagementTaskComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskManagementTaskComment_TaskManagementProjectUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "TaskManagementProjectUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskManagementTaskComment_TaskManagementTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskManagementTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementProjectUsers_MainAppUserId",
                table: "TaskManagementProjectUsers",
                column: "MainAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementProjectUsers_ProjectId",
                table: "TaskManagementProjectUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementTaskComment_CreatorId",
                table: "TaskManagementTaskComment",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementTaskComment_TaskId",
                table: "TaskManagementTaskComment",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementTasks_CreatorId",
                table: "TaskManagementTasks",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementTasks_ExecutorId",
                table: "TaskManagementTasks",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementTasks_ProjectId",
                table: "TaskManagementTasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementTasks_StatusId",
                table: "TaskManagementTasks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskManagementTaskStatus_ProjectId",
                table: "TaskManagementTaskStatus",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskManagementTaskComment");

            migrationBuilder.DropTable(
                name: "TaskManagementTasks");

            migrationBuilder.DropTable(
                name: "TaskManagementProjectUsers");

            migrationBuilder.DropTable(
                name: "TaskManagementTaskStatus");

            migrationBuilder.DropTable(
                name: "TaskManagementProjects");

            migrationBuilder.CreateTable(
                name: "ReviewProject",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewProjectUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deactivated = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    MainAppUserId = table.Column<long>(type: "bigint", nullable: true),
                    NotifyEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "TaskReviewStatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskReviewStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskReviewStatus_ReviewProject_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ReviewProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewTasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorEntityId = table.Column<long>(type: "bigint", nullable: false),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    ReviewerId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    StatusId = table.Column<long>(type: "bigint", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_ReviewTasks_TaskReviewStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "TaskReviewStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReviewComment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false),
                    TaskId = table.Column<long>(type: "bigint", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_ReviewTasks_StatusId",
                table: "ReviewTasks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskReviewStatus_ProjectId",
                table: "TaskReviewStatus",
                column: "ProjectId");
        }
    }
}
