using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Upward.Migrations
{
    public partial class ReInitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pkgapikey",
                columns: table => new
                {
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    project = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pkgapikey", x => x.key);
                });

            migrationBuilder.CreateTable(
                name: "pkgfile",
                columns: table => new
                {
                    id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    filename = table.Column<string[]>(type: "varchar(255)[]", nullable: false),
                    label = table.Column<string>(type: "varchar(39)", maxLength: 39, nullable: true),
                    project = table.Column<int>(type: "int4", nullable: false),
                    sha256 = table.Column<string[]>(type: "varchar(64)[]", nullable: false),
                    version = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pkgfile", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    name = table.Column<string>(type: "varchar(39)", maxLength: 39, nullable: false),
                    @private = table.Column<bool>(name: "private", type: "bool", nullable: false),
                    userid = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userprofile",
                columns: table => new
                {
                    id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    email = table.Column<string>(type: "varchar(320)", maxLength: 320, nullable: false),
                    password = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    username = table.Column<string>(type: "varchar(39)", maxLength: 39, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userprofile", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pkgapikey");

            migrationBuilder.DropTable(
                name: "pkgfile");

            migrationBuilder.DropTable(
                name: "project");

            migrationBuilder.DropTable(
                name: "userprofile");
        }
    }
}
