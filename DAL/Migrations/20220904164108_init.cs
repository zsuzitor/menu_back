using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DAL.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainLogTable",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnteredDate = table.Column<DateTime>(nullable: true, defaultValueSql: "now()"),
                    LogDate = table.Column<string>(nullable: true),
                    LogLevel = table.Column<string>(nullable: true),
                    LogLogger = table.Column<string>(nullable: true),
                    LogMessage = table.Column<string>(nullable: true),
                    LogException = table.Column<string>(nullable: true),
                    LogStacktrace = table.Column<string>(nullable: true),
                    ActionDateStart = table.Column<string>(nullable: true),
                    ActionDateEnd = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ConnectionId = table.Column<string>(nullable: true),
                    GroupName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainLogTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaningRooms",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaningRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewProject",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Login = table.Column<string>(nullable: false),
                    ImagePath = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: false),
                    RefreshTokenHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaningStories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Vote = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Completed = table.Column<bool>(nullable: false),
                    RoomId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaningStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaningStories_PlaningRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "PlaningRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    MainImagePath = table.Column<string>(nullable: true),
                    Followed = table.Column<bool>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaningRoomUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Roles = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    MainAppUserId = table.Column<long>(nullable: false),
                    RoomId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaningRoomUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaningRoomUsers_Users_MainAppUserId",
                        column: x => x.MainAppUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaningRoomUsers_PlaningRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "PlaningRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewProjectUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                name: "WordsCards",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImagePath = table.Column<string>(nullable: true),
                    Word = table.Column<string>(nullable: true),
                    WordAnswer = table.Column<string>(nullable: true),
                    Hided = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordsCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordsCards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WordsLists",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordsLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordsLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Path = table.Column<string>(nullable: true),
                    ArticleId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewTasks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    CreatorEntityId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false),
                    ReviewerId = table.Column<long>(nullable: true)
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
                name: "WordCardWordList",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WordCardId = table.Column<long>(nullable: false),
                    WordsListId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordCardWordList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordCardWordList_WordsCards_WordCardId",
                        column: x => x.WordCardId,
                        principalTable: "WordsCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordCardWordList_WordsLists_WordsListId",
                        column: x => x.WordsListId,
                        principalTable: "WordsLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewComment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                name: "IX_Articles_UserId",
                table: "Articles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ArticleId",
                table: "Images",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MainLogTable_EnteredDate",
                table: "MainLogTable",
                column: "EnteredDate");

            migrationBuilder.CreateIndex(
                name: "IX_PlaningRooms_Name",
                table: "PlaningRooms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaningRoomUsers_MainAppUserId",
                table: "PlaningRoomUsers",
                column: "MainAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaningRoomUsers_RoomId",
                table: "PlaningRoomUsers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaningStories_RoomId",
                table: "PlaningStories",
                column: "RoomId");

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
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordCardWordList_WordCardId",
                table: "WordCardWordList",
                column: "WordCardId");

            migrationBuilder.CreateIndex(
                name: "IX_WordCardWordList_WordsListId",
                table: "WordCardWordList",
                column: "WordsListId");

            migrationBuilder.CreateIndex(
                name: "IX_WordsCards_UserId",
                table: "WordsCards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WordsLists_UserId",
                table: "WordsLists",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "MainLogTable");

            migrationBuilder.DropTable(
                name: "PlaningRoomUsers");

            migrationBuilder.DropTable(
                name: "PlaningStories");

            migrationBuilder.DropTable(
                name: "ReviewComment");

            migrationBuilder.DropTable(
                name: "WordCardWordList");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "PlaningRooms");

            migrationBuilder.DropTable(
                name: "ReviewTasks");

            migrationBuilder.DropTable(
                name: "WordsCards");

            migrationBuilder.DropTable(
                name: "WordsLists");

            migrationBuilder.DropTable(
                name: "ReviewProjectUsers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ReviewProject");
        }
    }
}
