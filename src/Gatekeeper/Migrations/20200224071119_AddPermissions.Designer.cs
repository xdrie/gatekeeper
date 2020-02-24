﻿// <auto-generated />
using System;
using Gatekeeper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gatekeeper.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200224071119_AddPermissions")]
    partial class AddPermissions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2");

            modelBuilder.Entity("Gatekeeper.Models.Access.Permission", b =>
                {
                    b.Property<int>("dbid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Userdbid")
                        .HasColumnType("INTEGER");

                    b.Property<string>("path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("dbid");

                    b.HasIndex("Userdbid");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Gatekeeper.Models.Identity.CryptSecret", b =>
                {
                    b.Property<int>("dbid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("hash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("iterations")
                        .HasColumnType("INTEGER");

                    b.Property<int>("length")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("salt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("saltLength")
                        .HasColumnType("INTEGER");

                    b.HasKey("dbid");

                    b.ToTable("CryptSecret");
                });

            modelBuilder.Entity("Gatekeeper.Models.Identity.Token", b =>
                {
                    b.Property<int>("dbid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("expires")
                        .HasColumnType("TEXT");

                    b.Property<string>("scope")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("userdbid")
                        .HasColumnType("INTEGER");

                    b.HasKey("dbid");

                    b.HasIndex("content")
                        .IsUnique();

                    b.HasIndex("userdbid");

                    b.ToTable("tokens");
                });

            modelBuilder.Entity("Gatekeeper.Models.Identity.User", b =>
                {
                    b.Property<int>("dbid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("emailVisible")
                        .HasColumnType("INTEGER");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("passworddbid")
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
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("uuid")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("verification")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("dbid");

                    b.HasIndex("email")
                        .IsUnique();

                    b.HasIndex("passworddbid");

                    b.HasIndex("username")
                        .IsUnique();

                    b.HasIndex("uuid")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("Gatekeeper.Models.Access.Permission", b =>
                {
                    b.HasOne("Gatekeeper.Models.Identity.User", null)
                        .WithMany("permissions")
                        .HasForeignKey("Userdbid");
                });

            modelBuilder.Entity("Gatekeeper.Models.Identity.Token", b =>
                {
                    b.HasOne("Gatekeeper.Models.Identity.User", "user")
                        .WithMany()
                        .HasForeignKey("userdbid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Gatekeeper.Models.Identity.User", b =>
                {
                    b.HasOne("Gatekeeper.Models.Identity.CryptSecret", "password")
                        .WithMany()
                        .HasForeignKey("passworddbid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
