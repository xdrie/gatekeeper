using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gatekeeper.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: false),
                    username = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    emailPublic = table.Column<bool>(nullable: false),
                    totp = table.Column<byte[]>(nullable: true),
                    pronouns = table.Column<int>(nullable: false),
                    role = table.Column<int>(nullable: false),
                    verification = table.Column<string>(nullable: false),
                    registered = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    content = table.Column<string>(nullable: false),
                    userid = table.Column<int>(nullable: false),
                    expires = table.Column<DateTime>(nullable: false),
                    scope = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_tokens_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users1",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    salt = table.Column<byte[]>(nullable: false),
                    hash = table.Column<byte[]>(nullable: false),
                    iterations = table.Column<int>(nullable: false),
                    length = table.Column<int>(nullable: false),
                    saltLength = table.Column<int>(nullable: false),
                    Userid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users1", x => x.id);
                    table.ForeignKey(
                        name: "FK_users1_users_Userid",
                        column: x => x.Userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tokens_userid",
                table: "tokens",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_users1_Userid",
                table: "users1",
                column: "Userid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropTable(
                name: "users1");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
