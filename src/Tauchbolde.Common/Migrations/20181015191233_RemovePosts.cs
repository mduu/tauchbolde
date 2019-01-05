using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tauchbolde.Common.Migrations
{
    public partial class RemovePosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostImages");

            migrationBuilder.DropTable(
                name: "Posts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false),
                    Category = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IntroImageId = table.Column<Guid>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Diver_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Diver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Caption = table.Column<string>(nullable: true),
                    ImageUrlLarge = table.Column<string>(nullable: false),
                    ImageUrlThumbnail = table.Column<string>(nullable: true),
                    PostId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostImages_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_PostId",
                table: "PostImages",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Category_PublishDate",
                table: "Posts",
                columns: new[] { "Category", "PublishDate" });
        }
    }
}
