using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContentItem_ContentItem_ContentItemId",
                table: "UserContentItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentItem",
                table: "ContentItem");

            migrationBuilder.RenameTable(
                name: "ContentItem",
                newName: "ContentItems");

            migrationBuilder.RenameColumn(
                name: "Discriminator",
                table: "ContentItems",
                newName: "ContentType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentItems",
                table: "ContentItems",
                column: "ContentItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserContentItem_ContentItems_ContentItemId",
                table: "UserContentItem",
                column: "ContentItemId",
                principalTable: "ContentItems",
                principalColumn: "ContentItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContentItem_ContentItems_ContentItemId",
                table: "UserContentItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentItems",
                table: "ContentItems");

            migrationBuilder.RenameTable(
                name: "ContentItems",
                newName: "ContentItem");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "ContentItem",
                newName: "Discriminator");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentItem",
                table: "ContentItem",
                column: "ContentItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserContentItem_ContentItem_ContentItemId",
                table: "UserContentItem",
                column: "ContentItemId",
                principalTable: "ContentItem",
                principalColumn: "ContentItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
