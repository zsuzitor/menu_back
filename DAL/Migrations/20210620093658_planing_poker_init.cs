using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class planing_poker_init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlaningRooms",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaningRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaningRoomUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainAppUserId = table.Column<long>(nullable: false),
                    Roles = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
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
                name: "PlaningStories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Vote = table.Column<double>(nullable: false),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaningRoomUsers");

            migrationBuilder.DropTable(
                name: "PlaningStories");

            migrationBuilder.DropTable(
                name: "PlaningRooms");
        }
    }
}
