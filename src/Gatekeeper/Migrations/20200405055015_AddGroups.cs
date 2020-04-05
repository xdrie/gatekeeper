using Microsoft.EntityFrameworkCore.Migrations;

namespace Gatekeeper.Migrations
{
    public partial class AddGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.AddColumn<string>(
                name: "groupList",
                table: "users",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "groupList",
                table: "users");

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    dbid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Userdbid = table.Column<int>(type: "INTEGER", nullable: true),
                    path = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.dbid);
                    table.ForeignKey(
                        name: "FK_Permission_users_Userdbid",
                        column: x => x.Userdbid,
                        principalTable: "users",
                        principalColumn: "dbid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Userdbid",
                table: "Permission",
                column: "Userdbid");
        }
    }
}
