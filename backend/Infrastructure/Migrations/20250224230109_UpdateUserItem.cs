using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsArticles",
                table: "NewsArticles");

            migrationBuilder.RenameTable(
                name: "NewsArticles",
                newName: "ContentItem");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ContentItem",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentItem",
                table: "ContentItem",
                column: "ContentItemId");

            migrationBuilder.CreateTable(
                name: "UserContentItem",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ContentItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContentItem", x => new { x.UserId, x.ContentItemId });
                    table.ForeignKey(
                        name: "FK_UserContentItem_ContentItem_ContentItemId",
                        column: x => x.ContentItemId,
                        principalTable: "ContentItem",
                        principalColumn: "ContentItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContentItem_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserContentItem_ContentItemId",
                table: "UserContentItem",
                column: "ContentItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserContentItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentItem",
                table: "ContentItem");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ContentItem");

            migrationBuilder.RenameTable(
                name: "ContentItem",
                newName: "NewsArticles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsArticles",
                table: "NewsArticles",
                column: "ContentItemId");
        }
    }
}
