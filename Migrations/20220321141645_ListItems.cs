using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory_API.Migrations
{
    public partial class ListItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListItem_Items_ItemId",
                table: "ListItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ListItem_Lists_ListId",
                table: "ListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListItem",
                table: "ListItem");

            migrationBuilder.RenameTable(
                name: "ListItem",
                newName: "ListItems");

            migrationBuilder.RenameColumn(
                name: "ListId",
                table: "ListItems",
                newName: "ParentListId");

            migrationBuilder.RenameIndex(
                name: "IX_ListItem_ListId",
                table: "ListItems",
                newName: "IX_ListItems_ParentListId");

            migrationBuilder.RenameIndex(
                name: "IX_ListItem_ItemId",
                table: "ListItems",
                newName: "IX_ListItems_ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListItems",
                table: "ListItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ListItems_Items_ItemId",
                table: "ListItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ListItems_Lists_ParentListId",
                table: "ListItems",
                column: "ParentListId",
                principalTable: "Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListItems_Items_ItemId",
                table: "ListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ListItems_Lists_ParentListId",
                table: "ListItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListItems",
                table: "ListItems");

            migrationBuilder.RenameTable(
                name: "ListItems",
                newName: "ListItem");

            migrationBuilder.RenameColumn(
                name: "ParentListId",
                table: "ListItem",
                newName: "ListId");

            migrationBuilder.RenameIndex(
                name: "IX_ListItems_ParentListId",
                table: "ListItem",
                newName: "IX_ListItem_ListId");

            migrationBuilder.RenameIndex(
                name: "IX_ListItems_ItemId",
                table: "ListItem",
                newName: "IX_ListItem_ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListItem",
                table: "ListItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ListItem_Items_ItemId",
                table: "ListItem",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ListItem_Lists_ListId",
                table: "ListItem",
                column: "ListId",
                principalTable: "Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
