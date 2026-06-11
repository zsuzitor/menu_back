using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class projectuser1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskManagementTaskComment_TaskManagementProjectUsers_CreatorId",
                schema: "TaskManagementApp",
                table: "TaskManagementTaskComment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskManagementTasks_TaskManagementProjectUsers_CreatorId",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskManagementTasks_TaskManagementProjectUsers_ExecutorId",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskManagementWorkTimeLog_TaskManagementProjectUsers_ProjectUserId",
                schema: "TaskManagementApp",
                table: "TaskManagementWorkTimeLog");

            migrationBuilder.DropColumn(
                name: "CreatorEntityId",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks");

            migrationBuilder.DropColumn(
                name: "NotifyEmail",
                schema: "TaskManagementApp",
                table: "TaskManagementProjectUsers");

            migrationBuilder.DropColumn(
                name: "UserName",
                schema: "TaskManagementApp",
                table: "TaskManagementProjectUsers");

            migrationBuilder.RenameColumn(
                name: "ProjectUserId",
                schema: "TaskManagementApp",
                table: "TaskManagementWorkTimeLog",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskManagementWorkTimeLog_ProjectUserId",
                schema: "TaskManagementApp",
                table: "TaskManagementWorkTimeLog",
                newName: "IX_TaskManagementWorkTimeLog_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskManagementTaskComment_Users_CreatorId",
                schema: "TaskManagementApp",
                table: "TaskManagementTaskComment",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskManagementTasks_Users_CreatorId",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskManagementTasks_Users_ExecutorId",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks",
                column: "ExecutorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskManagementWorkTimeLog_Users_UserId",
                schema: "TaskManagementApp",
                table: "TaskManagementWorkTimeLog",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskManagementTaskComment_Users_CreatorId",
                schema: "TaskManagementApp",
                table: "TaskManagementTaskComment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskManagementTasks_Users_CreatorId",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskManagementTasks_Users_ExecutorId",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskManagementWorkTimeLog_Users_UserId",
                schema: "TaskManagementApp",
                table: "TaskManagementWorkTimeLog");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "TaskManagementApp",
                table: "TaskManagementWorkTimeLog",
                newName: "ProjectUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskManagementWorkTimeLog_UserId",
                schema: "TaskManagementApp",
                table: "TaskManagementWorkTimeLog",
                newName: "IX_TaskManagementWorkTimeLog_ProjectUserId");

            migrationBuilder.AddColumn<long>(
                name: "CreatorEntityId",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "NotifyEmail",
                schema: "TaskManagementApp",
                table: "TaskManagementProjectUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                schema: "TaskManagementApp",
                table: "TaskManagementProjectUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskManagementTaskComment_TaskManagementProjectUsers_CreatorId",
                schema: "TaskManagementApp",
                table: "TaskManagementTaskComment",
                column: "CreatorId",
                principalSchema: "TaskManagementApp",
                principalTable: "TaskManagementProjectUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskManagementTasks_TaskManagementProjectUsers_CreatorId",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks",
                column: "CreatorId",
                principalSchema: "TaskManagementApp",
                principalTable: "TaskManagementProjectUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskManagementTasks_TaskManagementProjectUsers_ExecutorId",
                schema: "TaskManagementApp",
                table: "TaskManagementTasks",
                column: "ExecutorId",
                principalSchema: "TaskManagementApp",
                principalTable: "TaskManagementProjectUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskManagementWorkTimeLog_TaskManagementProjectUsers_ProjectUserId",
                schema: "TaskManagementApp",
                table: "TaskManagementWorkTimeLog",
                column: "ProjectUserId",
                principalSchema: "TaskManagementApp",
                principalTable: "TaskManagementProjectUsers",
                principalColumn: "Id");
        }
    }
}
