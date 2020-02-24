using Microsoft.EntityFrameworkCore.Migrations;

namespace Gatekeeper.Migrations
{
    public partial class TwoFactorEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "totpEnabled",
                table: "users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "totpEnabled",
                table: "users");
        }
    }
}
