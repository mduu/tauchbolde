using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tauchbolde.Web.Migrations
{
    public partial class Change_Post_IntroImage_To_Guid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntroImage",
                table: "Posts");

            migrationBuilder.AddColumn<Guid>(
                name: "IntroImageId",
                table: "Posts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntroImageId",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "IntroImage",
                table: "Posts",
                nullable: true);
        }
    }
}
