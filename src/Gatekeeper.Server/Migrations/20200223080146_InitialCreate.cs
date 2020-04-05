using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gatekeeper.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CryptSecret",
                columns: table => new
                {
                    dbid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    salt = table.Column<byte[]>(nullable: false),
                    hash = table.Column<byte[]>(nullable: false),
                    iterations = table.Column<int>(nullable: false),
                    length = table.Column<int>(nullable: false),
                    saltLength = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptSecret", x => x.dbid);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    dbid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: false),
                    username = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    emailVisible = table.Column<bool>(nullable: false),
                    passworddbid = table.Column<int>(nullable: false),
                    totp = table.Column<byte[]>(nullable: true),
                    pronouns = table.Column<int>(nullable: false),
                    role = table.Column<int>(nullable: false),
                    verification = table.Column<string>(nullable: false),
                    registered = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.dbid);
                    table.ForeignKey(
                        name: "FK_users_CryptSecret_passworddbid",
                        column: x => x.passworddbid,
                        principalTable: "CryptSecret",
                        principalColumn: "dbid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    dbid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userdbid = table.Column<int>(nullable: false),
                    content = table.Column<string>(nullable: false),
                    expires = table.Column<DateTime>(nullable: false),
                    scope = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokens", x => x.dbid);
                    table.ForeignKey(
                        name: "FK_tokens_users_userdbid",
                        column: x => x.userdbid,
                        principalTable: "users",
                        principalColumn: "dbid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tokens_userdbid",
                table: "tokens",
                column: "userdbid");

            migrationBuilder.CreateIndex(
                name: "IX_users_passworddbid",
                table: "users",
                column: "passworddbid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "CryptSecret");
        }
    }
}
