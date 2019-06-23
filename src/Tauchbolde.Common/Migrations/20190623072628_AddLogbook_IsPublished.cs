using Microsoft.EntityFrameworkCore.Migrations;

namespace Tauchbolde.Common.Migrations
{
    public partial class AddLogbook_IsPublished : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "LogbookEntries",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "LogbookEntries");
        }
    }
}
