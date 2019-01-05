using Microsoft.EntityFrameworkCore.Migrations;

namespace Tauchbolde.Common.Migrations
{
    public partial class AddFacebookId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookId",
                table: "Diver",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookId",
                table: "Diver");
        }
    }
}
