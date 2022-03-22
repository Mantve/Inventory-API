using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory_API.Migrations
{
    public partial class reminders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientUsername = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AuthorUsername = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Contents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_AuthorUsername",
                        column: x => x.AuthorUsername,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_RecipientUsername",
                        column: x => x.RecipientUsername,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReminderTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Repeat = table.Column<bool>(type: "bit", nullable: false),
                    RepeatFrequency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reminders_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AuthorUsername",
                table: "Messages",
                column: "AuthorUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientUsername",
                table: "Messages",
                column: "RecipientUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_ItemId",
                table: "Reminders",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Reminders");
        }
    }
}
