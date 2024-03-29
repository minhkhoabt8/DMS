﻿// <auto-generated />
using System;
using Auth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Auth.API.Migrations
{
    [DbContext(typeof(AuthContext))]
    partial class AuthContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AccountRole", b =>
                {
                    b.Property<Guid>("AccountsID")
                        .HasColumnType("uuid");

                    b.Property<int>("RolesID")
                        .HasColumnType("integer");

                    b.HasKey("AccountsID", "RolesID");

                    b.HasIndex("RolesID");

                    b.ToTable("AccountRole");
                });

            modelBuilder.Entity("Auth.Core.Entities.Account", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Auth.Core.Entities.File", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("OwnerID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParentFolderID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.HasIndex("OwnerID");

                    b.HasIndex("ParentFolderID");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Auth.Core.Entities.FileShareRule", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("FileID")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsRoot")
                        .HasColumnType("boolean");

                    b.Property<int>("Scope")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("FileID");

                    b.ToTable("FileShareRule");
                });

            modelBuilder.Entity("Auth.Core.Entities.Folder", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("OwnerID")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ParentFolderID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.HasIndex("OwnerID");

                    b.HasIndex("ParentFolderID");

                    b.ToTable("Folders");
                });

            modelBuilder.Entity("Auth.Core.Entities.FolderShareRule", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("FolderID")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsRoot")
                        .HasColumnType("boolean");

                    b.Property<int>("Scope")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("FolderID");

                    b.ToTable("FolderShareRule");
                });

            modelBuilder.Entity("Auth.Core.Entities.RefreshToken", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<Guid>("AccountID")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("boolean");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Auth.Core.Entities.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("ID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("AccountRole", b =>
                {
                    b.HasOne("Auth.Core.Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Auth.Core.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Auth.Core.Entities.File", b =>
                {
                    b.HasOne("Auth.Core.Entities.Account", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Auth.Core.Entities.Folder", "ParentFolder")
                        .WithMany("Files")
                        .HasForeignKey("ParentFolderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("ParentFolder");
                });

            modelBuilder.Entity("Auth.Core.Entities.FileShareRule", b =>
                {
                    b.HasOne("Auth.Core.Entities.File", "File")
                        .WithMany("ShareRules")
                        .HasForeignKey("FileID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");
                });

            modelBuilder.Entity("Auth.Core.Entities.Folder", b =>
                {
                    b.HasOne("Auth.Core.Entities.Account", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Auth.Core.Entities.Folder", "ParentFolder")
                        .WithMany("SubFolders")
                        .HasForeignKey("ParentFolderID");

                    b.Navigation("Owner");

                    b.Navigation("ParentFolder");
                });

            modelBuilder.Entity("Auth.Core.Entities.FolderShareRule", b =>
                {
                    b.HasOne("Auth.Core.Entities.Folder", "Folder")
                        .WithMany("ShareRules")
                        .HasForeignKey("FolderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Folder");
                });

            modelBuilder.Entity("Auth.Core.Entities.RefreshToken", b =>
                {
                    b.HasOne("Auth.Core.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Auth.Core.Entities.File", b =>
                {
                    b.Navigation("ShareRules");
                });

            modelBuilder.Entity("Auth.Core.Entities.Folder", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("ShareRules");

                    b.Navigation("SubFolders");
                });
#pragma warning restore 612, 618
        }
    }
}
