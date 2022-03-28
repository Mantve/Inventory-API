using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory_API.Migrations
{
    public partial class FriendsRelationFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_Username1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username1",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserUser",
                columns: table => new
                {
                    FriendOfUsername = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    FriendsUsername = table.Column<string>(type: "nvarchar(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUser", x => new { x.FriendOfUsername, x.FriendsUsername });
                    table.ForeignKey(
                        name: "FK_UserUser_Users_FriendOfUsername",
                        column: x => x.FriendOfUsername,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUser_Users_FriendsUsername",
                        column: x => x.FriendsUsername,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserUser_FriendsUsername",
                table: "UserUser",
                column: "FriendsUsername");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserUser");

            migrationBuilder.AddColumn<string>(
                name: "Username1",
                table: "Users",
                type: "nvarchar(32)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username1",
                table: "Users",
                column: "Username1");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_Username1",
                table: "Users",
                column: "Username1",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
