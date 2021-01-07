﻿// <auto-generated />
using System;
using Gatekeeper.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Gatekeeper.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "3.1.9");

            modelBuilder.Entity("Gatekeeper.Models.Identity.CryptSecret", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("hash")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("CryptSecret");
                });

            modelBuilder.Entity("Gatekeeper.Models.Identity.Token", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("content")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("expires")
                        .HasColumnType("TEXT");

                    b.Property<string>("scope")
                        .HasColumnType("TEXT");

                    b.Property<int?>("userid")
                        .HasColumnType("INTEGER");

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
                        .HasColumnType("INTEGER");

                    b.Property<string>("email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("emailVisible")
                        .HasColumnType("INTEGER");

                    b.Property<string>("groupList")
                        .HasColumnType("TEXT");

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("passwordid")
                        .HasColumnType("INTEGER");

                    b.Property<int>("pronouns")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("registered")
                        .HasColumnType("TEXT");

                    b.Property<int>("role")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("totp")
                        .HasColumnType("BLOB");

                    b.Property<bool>("totpEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("username")
                        .HasColumnType("TEXT");

                    b.Property<string>("uuid")
                        .HasColumnType("TEXT");

                    b.Property<string>("verification")
                        .HasColumnType("TEXT");

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
