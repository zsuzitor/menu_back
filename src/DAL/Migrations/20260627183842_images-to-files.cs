using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class imagestofiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Images_ImageId",
                schema: "MenuApp",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaningRooms_Images_ImageId",
                schema: "PlaningPoker",
                table: "PlaningRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_ImageId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_WordsCards_Images_ImageId",
                schema: "WordsCards",
                table: "WordsCards");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArticleId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PhysFileShouldBeDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PhysFileDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "MenuApp",
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_ArticleId",
                table: "Files",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Files_ImageId",
                schema: "MenuApp",
                table: "Articles",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaningRooms_Files_ImageId",
                schema: "PlaningPoker",
                table: "PlaningRooms",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Files_ImageId",
                table: "Users",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WordsCards_Files_ImageId",
                schema: "WordsCards",
                table: "WordsCards",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Files_ImageId",
                schema: "MenuApp",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaningRooms_Files_ImageId",
                schema: "PlaningPoker",
                table: "PlaningRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Files_ImageId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_WordsCards_Files_ImageId",
                schema: "WordsCards",
                table: "WordsCards");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhysFileDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PhysFileShouldBeDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "MenuApp",
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_ArticleId",
                table: "Images",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Images_ImageId",
                schema: "MenuApp",
                table: "Articles",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaningRooms_Images_ImageId",
                schema: "PlaningPoker",
                table: "PlaningRooms",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Images_ImageId",
                table: "Users",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WordsCards_Images_ImageId",
                schema: "WordsCards",
                table: "WordsCards",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
