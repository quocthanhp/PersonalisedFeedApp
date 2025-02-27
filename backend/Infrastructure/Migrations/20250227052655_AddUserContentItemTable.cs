using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserContentItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContentItem_ContentItems_ContentItemId",
                table: "UserContentItem");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContentItem_User_UserId",
                table: "UserContentItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserContentItem",
                table: "UserContentItem");

            migrationBuilder.RenameTable(
                name: "UserContentItem",
                newName: "UserContentItems");

            migrationBuilder.RenameIndex(
                name: "IX_UserContentItem_ContentItemId",
                table: "UserContentItems",
                newName: "IX_UserContentItems_ContentItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserContentItems",
                table: "UserContentItems",
                columns: new[] { "UserId", "ContentItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserContentItems_ContentItems_ContentItemId",
                table: "UserContentItems",
                column: "ContentItemId",
                principalTable: "ContentItems",
                principalColumn: "ContentItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContentItems_User_UserId",
                table: "UserContentItems",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContentItems_ContentItems_ContentItemId",
                table: "UserContentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContentItems_User_UserId",
                table: "UserContentItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserContentItems",
                table: "UserContentItems");

            migrationBuilder.RenameTable(
                name: "UserContentItems",
                newName: "UserContentItem");

            migrationBuilder.RenameIndex(
                name: "IX_UserContentItems_ContentItemId",
                table: "UserContentItem",
                newName: "IX_UserContentItem_ContentItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserContentItem",
                table: "UserContentItem",
                columns: new[] { "UserId", "ContentItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserContentItem_ContentItems_ContentItemId",
                table: "UserContentItem",
                column: "ContentItemId",
                principalTable: "ContentItems",
                principalColumn: "ContentItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContentItem_User_UserId",
                table: "UserContentItem",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
