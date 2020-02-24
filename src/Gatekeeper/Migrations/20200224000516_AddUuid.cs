using Microsoft.EntityFrameworkCore.Migrations;

namespace Gatekeeper.Migrations
{
    public partial class AddUuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "uuid",
                table: "users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_users_username_email_uuid",
                table: "users",
                columns: new[] { "username", "email", "uuid" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_username_email_uuid",
                table: "users");

            migrationBuilder.DropColumn(
                name: "uuid",
                table: "users");
        }
    }
}
