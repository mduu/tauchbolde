using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tauchbolde.Web.Migrations
{
    public partial class Change_Post_Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_Posts_Category_PublishDate", "Posts");
            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(string));
            migrationBuilder.CreateIndex(
                name: "IX_Posts_Category_PublishDate",
                table: "Posts",
                columns: new[] { "Category", "PublishDate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_Posts_Category_PublishDate", "Posts");
            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(int));
            migrationBuilder.CreateIndex(
                name: "IX_Posts_Category_PublishDate",
                table: "Posts",
                columns: new[] { "Category", "PublishDate" });
        }
    }
}
