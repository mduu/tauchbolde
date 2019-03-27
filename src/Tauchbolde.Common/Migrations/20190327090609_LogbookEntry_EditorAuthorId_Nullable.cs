using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tauchbolde.Common.Migrations
{
    public partial class LogbookEntry_EditorAuthorId_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "EditorAuthorId",
                table: "LogbookEntries",
                nullable: true,
                oldClrType: typeof(Guid));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "EditorAuthorId",
                table: "LogbookEntries",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
