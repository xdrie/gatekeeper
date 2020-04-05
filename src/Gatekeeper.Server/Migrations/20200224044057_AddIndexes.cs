using Microsoft.EntityFrameworkCore.Migrations;

namespace Gatekeeper.Server.Migrations
{
    public partial class AddIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_username_email_uuid",
                table: "users");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_tokens_content",
                table: "tokens",
                column: "content",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_email",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_username",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_uuid",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_tokens_content",
                table: "tokens");

            migrationBuilder.CreateIndex(
                name: "IX_users_username_email_uuid",
                table: "users",
                columns: new[] { "username", "email", "uuid" },
                unique: true);
        }
    }
}
