using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Upward.Migrations
{
    public partial class RemoveUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "username",
                table: "userprofile");

            migrationBuilder.DropColumn(
                name: "userid",
                table: "pkgfile");

            migrationBuilder.DropColumn(
                name: "userid",
                table: "pkgapikey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "userprofile",
                maxLength: 39,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "userid",
                table: "pkgfile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userid",
                table: "pkgapikey",
                nullable: false,
                defaultValue: 0);
        }
    }
}
