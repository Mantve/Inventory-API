using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory_API.Migrations
{
    public partial class ParentItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Items_ItemId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Items",
                newName: "ParentItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_ItemId",
                table: "Items",
                newName: "IX_Items_ParentItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Items_ParentItemId",
                table: "Items",
                column: "ParentItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Items_ParentItemId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ParentItemId",
                table: "Items",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_ParentItemId",
                table: "Items",
                newName: "IX_Items_ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Items_ItemId",
                table: "Items",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
