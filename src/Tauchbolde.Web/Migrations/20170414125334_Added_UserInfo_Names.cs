using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tauchbolde.Web.Migrations
{
    public partial class Added_UserInfo_Names : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Firstname",
                table: "UserInfos",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fullname",
                table: "UserInfos",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nachname",
                table: "UserInfos",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Firstname",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Fullname",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Nachname",
                table: "UserInfos");
        }
    }
}
