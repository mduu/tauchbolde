using Microsoft.EntityFrameworkCore.Migrations;

namespace Tauchbolde.Common.Migrations
{
    public partial class Add_Diver_OwnNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SendOwnNoticiations",
                table: "Diver",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendOwnNoticiations",
                table: "Diver");
        }
    }
}
