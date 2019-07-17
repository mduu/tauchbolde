using Microsoft.EntityFrameworkCore.Migrations;

namespace Tauchbolde.DataAccess.Migrations
{
    public partial class AddAvatarId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarId",
                table: "Diver",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "Diver");
        }
    }
}
