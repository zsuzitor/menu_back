using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class newimageslogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "WordsCards");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "PlaningRooms");

            migrationBuilder.DropColumn(
                name: "MainImagePath",
                table: "Articles");

            migrationBuilder.AddColumn<long>(
                name: "ImageId",
                table: "WordsCards",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ImageId",
                table: "PlaningRooms",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ImageId",
                table: "Articles",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordsCards_ImageId",
                table: "WordsCards",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaningRooms_ImageId",
                table: "PlaningRooms",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ImageId",
                table: "Articles",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Images_ImageId",
                table: "Articles",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaningRooms_Images_ImageId",
                table: "PlaningRooms",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WordsCards_Images_ImageId",
                table: "WordsCards",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Images_ImageId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaningRooms_Images_ImageId",
                table: "PlaningRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_WordsCards_Images_ImageId",
                table: "WordsCards");

            migrationBuilder.DropIndex(
                name: "IX_WordsCards_ImageId",
                table: "WordsCards");

            migrationBuilder.DropIndex(
                name: "IX_PlaningRooms_ImageId",
                table: "PlaningRooms");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ImageId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "WordsCards");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "PlaningRooms");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "WordsCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "PlaningRooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainImagePath",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
