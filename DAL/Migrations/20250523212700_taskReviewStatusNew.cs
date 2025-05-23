using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class taskReviewStatusNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ReviewTasks");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "WordsLists",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "WordsCards",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "VaultUsers",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Vaults",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Users",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Secrets",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ReviewTasks",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StatusId",
                table: "ReviewTasks",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ReviewProjectUsers",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ReviewProject",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "PlaningStories",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "PlaningRoomUsers",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "PlaningRooms",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Images",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Articles",
                rowVersion: true,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskReviewStatus",
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
                    table.PrimaryKey("PK_TaskReviewStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskReviewStatus_ReviewProject_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ReviewProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewTasks_StatusId",
                table: "ReviewTasks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskReviewStatus_ProjectId",
                table: "TaskReviewStatus",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewTasks_TaskReviewStatus_StatusId",
                table: "ReviewTasks",
                column: "StatusId",
                principalTable: "TaskReviewStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewTasks_TaskReviewStatus_StatusId",
                table: "ReviewTasks");

            migrationBuilder.DropTable(
                name: "TaskReviewStatus");

            migrationBuilder.DropIndex(
                name: "IX_ReviewTasks_StatusId",
                table: "ReviewTasks");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "WordsLists");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "WordsCards");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "VaultUsers");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Vaults");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Secrets");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ReviewTasks");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "ReviewTasks");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ReviewProjectUsers");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ReviewProject");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "PlaningStories");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "PlaningRoomUsers");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "PlaningRooms");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ReviewTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
