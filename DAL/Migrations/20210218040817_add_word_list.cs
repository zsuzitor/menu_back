using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class add_word_list : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Articles_ArticleId",
                table: "Images");

            migrationBuilder.CreateTable(
                name: "WordsLists",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "WordCardWordList",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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

            migrationBuilder.CreateIndex(
                name: "IX_WordCardWordList_WordCardId",
                table: "WordCardWordList",
                column: "WordCardId");

            migrationBuilder.CreateIndex(
                name: "IX_WordCardWordList_WordsListId",
                table: "WordCardWordList",
                column: "WordsListId");

            migrationBuilder.CreateIndex(
                name: "IX_WordsLists_UserId",
                table: "WordsLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Articles_ArticleId",
                table: "Images",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Articles_ArticleId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "WordCardWordList");

            migrationBuilder.DropTable(
                name: "WordsLists");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Articles_ArticleId",
                table: "Images",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
