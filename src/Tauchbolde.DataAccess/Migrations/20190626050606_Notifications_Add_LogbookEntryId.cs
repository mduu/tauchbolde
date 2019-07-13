using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tauchbolde.DataAccess.Migrations
{
    public partial class Notifications_Add_LogbookEntryId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Events_EventId",
                table: "Notifications");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "Notifications",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "LogbookEntryId",
                table: "Notifications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_LogbookEntryId",
                table: "Notifications",
                column: "LogbookEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Events_EventId",
                table: "Notifications",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_LogbookEntries_LogbookEntryId",
                table: "Notifications",
                column: "LogbookEntryId",
                principalTable: "LogbookEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Events_EventId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_LogbookEntries_LogbookEntryId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_LogbookEntryId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "LogbookEntryId",
                table: "Notifications");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "Notifications",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Events_EventId",
                table: "Notifications",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
