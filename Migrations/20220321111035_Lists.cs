using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory_API.Migrations
{
    public partial class Lists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Lists_ListId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Lists_Users_Username",
                table: "Lists");

            migrationBuilder.DropIndex(
                name: "IX_Items_ListId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ListId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Lists",
                newName: "AuthorUsername");

            migrationBuilder.RenameIndex(
                name: "IX_Lists_Username",
                table: "Lists",
                newName: "IX_Lists_AuthorUsername");

            migrationBuilder.CreateTable(
                name: "ListItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    ListId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListItem_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ListItem_Lists_ListId",
                        column: x => x.ListId,
                        principalTable: "Lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListItem_ItemId",
                table: "ListItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ListItem_ListId",
                table: "ListItem",
                column: "ListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_Users_AuthorUsername",
                table: "Lists",
                column: "AuthorUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lists_Users_AuthorUsername",
                table: "Lists");

            migrationBuilder.DropTable(
                name: "ListItem");

            migrationBuilder.RenameColumn(
                name: "AuthorUsername",
                table: "Lists",
                newName: "Username");

            migrationBuilder.RenameIndex(
                name: "IX_Lists_AuthorUsername",
                table: "Lists",
                newName: "IX_Lists_Username");

            migrationBuilder.AddColumn<int>(
                name: "ListId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ListId",
                table: "Items",
                column: "ListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Lists_ListId",
                table: "Items",
                column: "ListId",
                principalTable: "Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_Users_Username",
                table: "Lists",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
