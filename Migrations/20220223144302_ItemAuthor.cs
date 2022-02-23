using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory_API.Migrations
{
    public partial class ItemAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_categoryId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "Items",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_categoryId",
                table: "Items",
                newName: "IX_Items_CategoryId");

            migrationBuilder.AddColumn<string>(
                name: "AuthorUsername",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_AuthorUsername",
                table: "Items",
                column: "AuthorUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_AuthorUsername",
                table: "Items",
                column: "AuthorUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_AuthorUsername",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_AuthorUsername",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "AuthorUsername",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Items",
                newName: "categoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                newName: "IX_Items_categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_categoryId",
                table: "Items",
                column: "categoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
