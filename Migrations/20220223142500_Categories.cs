using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory_API.Migrations
{
    public partial class Categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Category_categoryId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "AuthorUsername",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_AuthorUsername",
                table: "Categories",
                column: "AuthorUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_AuthorUsername",
                table: "Categories",
                column: "AuthorUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_categoryId",
                table: "Items",
                column: "categoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_AuthorUsername",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_categoryId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_AuthorUsername",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "AuthorUsername",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Category_categoryId",
                table: "Items",
                column: "categoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
