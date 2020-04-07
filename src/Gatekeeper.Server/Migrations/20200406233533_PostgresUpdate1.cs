using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Gatekeeper.Server.Migrations
{
    public partial class PostgresUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CryptSecret",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    salt = table.Column<byte[]>(nullable: true),
                    hash = table.Column<byte[]>(nullable: true),
                    iterations = table.Column<int>(nullable: false),
                    length = table.Column<int>(nullable: false),
                    saltLength = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptSecret", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    uuid = table.Column<string>(nullable: true),
                    emailVisible = table.Column<bool>(nullable: false),
                    passwordid = table.Column<int>(nullable: true),
                    totp = table.Column<byte[]>(nullable: true),
                    totpEnabled = table.Column<bool>(nullable: false),
                    pronouns = table.Column<int>(nullable: false),
                    role = table.Column<int>(nullable: false),
                    verification = table.Column<string>(nullable: true),
                    registered = table.Column<DateTime>(nullable: false),
                    groupList = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_CryptSecret_passwordid",
                        column: x => x.passwordid,
                        principalTable: "CryptSecret",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    userid = table.Column<int>(nullable: true),
                    content = table.Column<string>(nullable: true),
                    expires = table.Column<DateTime>(nullable: false),
                    scope = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_tokens_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tokens_content",
                table: "tokens",
                column: "content",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tokens_userid",
                table: "tokens",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_passwordid",
                table: "users",
                column: "passwordid");

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_uuid",
                table: "users",
                column: "uuid",
                unique: true);
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
