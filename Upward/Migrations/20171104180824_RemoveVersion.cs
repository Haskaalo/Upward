using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Upward.Migrations
{
    public partial class RemoveVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "version",
                table: "pkgfile");

            migrationBuilder.AddColumn<int>(
                name: "major",
                table: "pkgfile",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "minor",
                table: "pkgfile",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "patch",
                table: "pkgfile",
                type: "int4",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "major",
                table: "pkgfile");

            migrationBuilder.DropColumn(
                name: "minor",
                table: "pkgfile");

            migrationBuilder.DropColumn(
                name: "patch",
                table: "pkgfile");

            migrationBuilder.AddColumn<string>(
                name: "version",
                table: "pkgfile",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
