using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Gatekeeper.Server.Migrations
{
    public partial class PostgresUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tokens_users_userdbid",
                table: "tokens");

            migrationBuilder.DropForeignKey(
                name: "FK_users_CryptSecret_passworddbid",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_passworddbid",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tokens",
                table: "tokens");

            migrationBuilder.DropIndex(
                name: "IX_tokens_userdbid",
                table: "tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CryptSecret",
                table: "CryptSecret");

            migrationBuilder.DropColumn(
                name: "dbid",
                table: "users");

            migrationBuilder.DropColumn(
                name: "passworddbid",
                table: "users");

            migrationBuilder.DropColumn(
                name: "dbid",
                table: "tokens");

            migrationBuilder.DropColumn(
                name: "userdbid",
                table: "tokens");

            migrationBuilder.DropColumn(
                name: "dbid",
                table: "CryptSecret");

            migrationBuilder.AlterColumn<string>(
                name: "verification",
                table: "users",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "uuid",
                table: "users",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "users",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<bool>(
                name: "totpEnabled",
                table: "users",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<byte[]>(
                name: "totp",
                table: "users",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role",
                table: "users",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "registered",
                table: "users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "pronouns",
                table: "users",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "users",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "groupList",
                table: "users",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<bool>(
                name: "emailVisible",
                table: "users",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "users",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "users",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "passwordid",
                table: "users",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "scope",
                table: "tokens",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires",
                table: "tokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "tokens",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "tokens",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "userid",
                table: "tokens",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "saltLength",
                table: "CryptSecret",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<byte[]>(
                name: "salt",
                table: "CryptSecret",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");

            migrationBuilder.AlterColumn<int>(
                name: "length",
                table: "CryptSecret",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "iterations",
                table: "CryptSecret",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<byte[]>(
                name: "hash",
                table: "CryptSecret",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "CryptSecret",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tokens",
                table: "tokens",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CryptSecret",
                table: "CryptSecret",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_users_passwordid",
                table: "users",
                column: "passwordid");

            migrationBuilder.CreateIndex(
                name: "IX_tokens_userid",
                table: "tokens",
                column: "userid");

            migrationBuilder.AddForeignKey(
                name: "FK_tokens_users_userid",
                table: "tokens",
                column: "userid",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_CryptSecret_passwordid",
                table: "users",
                column: "passwordid",
                principalTable: "CryptSecret",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tokens_users_userid",
                table: "tokens");

            migrationBuilder.DropForeignKey(
                name: "FK_users_CryptSecret_passwordid",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_passwordid",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tokens",
                table: "tokens");

            migrationBuilder.DropIndex(
                name: "IX_tokens_userid",
                table: "tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CryptSecret",
                table: "CryptSecret");

            migrationBuilder.DropColumn(
                name: "id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "passwordid",
                table: "users");

            migrationBuilder.DropColumn(
                name: "id",
                table: "tokens");

            migrationBuilder.DropColumn(
                name: "userid",
                table: "tokens");

            migrationBuilder.DropColumn(
                name: "id",
                table: "CryptSecret");

            migrationBuilder.AlterColumn<string>(
                name: "verification",
                table: "users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "uuid",
                table: "users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "totpEnabled",
                table: "users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<byte[]>(
                name: "totp",
                table: "users",
                type: "BLOB",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role",
                table: "users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "registered",
                table: "users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<int>(
                name: "pronouns",
                table: "users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "groupList",
                table: "users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "emailVisible",
                table: "users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "dbid",
                table: "users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "passworddbid",
                table: "users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "scope",
                table: "tokens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "expires",
                table: "tokens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "tokens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "dbid",
                table: "tokens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userdbid",
                table: "tokens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "saltLength",
                table: "CryptSecret",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<byte[]>(
                name: "salt",
                table: "CryptSecret",
                type: "BLOB",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "length",
                table: "CryptSecret",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "iterations",
                table: "CryptSecret",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<byte[]>(
                name: "hash",
                table: "CryptSecret",
                type: "BLOB",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "dbid",
                table: "CryptSecret",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "dbid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tokens",
                table: "tokens",
                column: "dbid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CryptSecret",
                table: "CryptSecret",
                column: "dbid");

            migrationBuilder.CreateIndex(
                name: "IX_users_passworddbid",
                table: "users",
                column: "passworddbid");

            migrationBuilder.CreateIndex(
                name: "IX_tokens_userdbid",
                table: "tokens",
                column: "userdbid");

            migrationBuilder.AddForeignKey(
                name: "FK_tokens_users_userdbid",
                table: "tokens",
                column: "userdbid",
                principalTable: "users",
                principalColumn: "dbid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_CryptSecret_passworddbid",
                table: "users",
                column: "passworddbid",
                principalTable: "CryptSecret",
                principalColumn: "dbid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
