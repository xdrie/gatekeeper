using Microsoft.EntityFrameworkCore.Migrations;

namespace Gatekeeper.Migrations
{
    public partial class AddPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    dbid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    path = table.Column<string>(nullable: false),
                    Userdbid = table.Column<int>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permission");
        }
    }
}
