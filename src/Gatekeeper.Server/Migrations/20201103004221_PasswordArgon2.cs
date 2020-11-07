using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gatekeeper.Server.Migrations
{
    public partial class PasswordArgon2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "iterations",
                table: "CryptSecret");

            migrationBuilder.DropColumn(
                name: "length",
                table: "CryptSecret");

            migrationBuilder.DropColumn(
                name: "salt",
                table: "CryptSecret");

            migrationBuilder.DropColumn(
                name: "saltLength",
                table: "CryptSecret");

            migrationBuilder.AlterColumn<string>(
                name: "hash",
                table: "CryptSecret",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "hash",
                table: "CryptSecret",
                type: "BLOB",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "iterations",
                table: "CryptSecret",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "length",
                table: "CryptSecret",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "salt",
                table: "CryptSecret",
                type: "BLOB",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "saltLength",
                table: "CryptSecret",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
