using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tauchbolde.Driver.DataAccessSql.Migrations
{
    public partial class Add_LogbookEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogbookEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    TeaserText = table.Column<string>(nullable: true),
                    IsFavorite = table.Column<bool>(nullable: false),
                    TeaserImage = table.Column<string>(nullable: true),
                    TeaserImageThumb = table.Column<string>(nullable: true),
                    ExternalPhotoAlbumUrl = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    EditorAuthorId = table.Column<Guid>(nullable: false),
                    OriginalAuthorId = table.Column<Guid>(nullable: false),
                    EventId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogbookEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogbookEntries_Diver_EditorAuthorId",
                        column: x => x.EditorAuthorId,
                        principalTable: "Diver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogbookEntries_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogbookEntries_Diver_OriginalAuthorId",
                        column: x => x.OriginalAuthorId,
                        principalTable: "Diver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogbookEntries_EditorAuthorId",
                table: "LogbookEntries",
                column: "EditorAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_LogbookEntries_EventId",
                table: "LogbookEntries",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_LogbookEntries_OriginalAuthorId",
                table: "LogbookEntries",
                column: "OriginalAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_LogbookEntries_IsFavorite_CreatedAt",
                table: "LogbookEntries",
                columns: new[] { "IsFavorite", "CreatedAt" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogbookEntries");
        }
    }
}
