using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStoreApplication.Migrations
{
    public partial class AddingNewBookForeignKeyInBooksGallery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BooksGallery_Books_BooksID",
                table: "BooksGallery");

            migrationBuilder.DropIndex(
                name: "IX_BooksGallery_BooksID",
                table: "BooksGallery");

            migrationBuilder.DropColumn(
                name: "BooksID",
                table: "BooksGallery");

            migrationBuilder.CreateIndex(
                name: "IX_BooksGallery_BookID",
                table: "BooksGallery",
                column: "BookID");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksGallery_Books_BookID",
                table: "BooksGallery",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BooksGallery_Books_BookID",
                table: "BooksGallery");

            migrationBuilder.DropIndex(
                name: "IX_BooksGallery_BookID",
                table: "BooksGallery");

            migrationBuilder.AddColumn<int>(
                name: "BooksID",
                table: "BooksGallery",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BooksGallery_BooksID",
                table: "BooksGallery",
                column: "BooksID");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksGallery_Books_BooksID",
                table: "BooksGallery",
                column: "BooksID",
                principalTable: "Books",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
