﻿// <auto-generated />
using System;
using Gatekeeper.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Gatekeeper.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200406233533_PostgresUpdate1")]
    partial class PostgresUpdate1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Gatekeeper.Models.Identity.CryptSecret", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<byte[]>("hash")
                        .HasColumnType("bytea");

                    b.Property<int>("iterations")
                        .HasColumnType("integer");

                    b.Property<int>("length")
                        .HasColumnType("integer");

                    b.Property<byte[]>("salt")
                        .HasColumnType("bytea");

                    b.Property<int>("saltLength")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("CryptSecret");
                });

            modelBuilder.Entity("Gatekeeper.Models.Identity.Token", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("content")
                        .HasColumnType("text");

                    b.Property<DateTime>("expires")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("scope")
                        .HasColumnType("text");

                    b.Property<int?>("userid")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("content")
                        .IsUnique();

                    b.HasIndex("userid");

                    b.ToTable("tokens");
                });

            modelBuilder.Entity("Gatekeeper.Models.Identity.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<bool>("emailVisible")
                        .HasColumnType("boolean");

                    b.Property<string>("groupList")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<int?>("passwordid")
                        .HasColumnType("integer");

                    b.Property<int>("pronouns")
                        .HasColumnType("integer");

                    b.Property<DateTime>("registered")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("role")
                        .HasColumnType("integer");

                    b.Property<byte[]>("totp")
                        .HasColumnType("bytea");

                    b.Property<bool>("totpEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("username")
                        .HasColumnType("text");

                    b.Property<string>("uuid")
                        .HasColumnType("text");

                    b.Property<string>("verification")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("email")
                        .IsUnique();

                    b.HasIndex("passwordid");

                    b.HasIndex("username")
                        .IsUnique();

                    b.HasIndex("uuid")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("Gatekeeper.Models.Identity.Token", b =>
                {
                    b.HasOne("Gatekeeper.Models.Identity.User", "user")
                        .WithMany()
                        .HasForeignKey("userid");
                });

            modelBuilder.Entity("Gatekeeper.Models.Identity.User", b =>
                {
                    b.HasOne("Gatekeeper.Models.Identity.CryptSecret", "password")
                        .WithMany()
                        .HasForeignKey("passwordid");
                });
#pragma warning restore 612, 618
        }
    }
}