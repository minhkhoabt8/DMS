﻿// <auto-generated />
using System;
using Content.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Content.API.Migrations
{
    [DbContext(typeof(ContentContext))]
    [Migration("20220124021232_FileVersion_UploadedAt")]
    partial class FileVersion_UploadedAt
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Content.Core.Entities.Account", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("Content.Core.Entities.File", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("OwnerID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.HasIndex("OwnerID");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Content.Core.Entities.FileVersion", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int?>("BaseVersionID")
                        .HasColumnType("integer");

                    b.Property<string>("DeltaUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("FileID")
                        .HasColumnType("uuid");

                    b.Property<int>("FileName")
                        .HasColumnType("integer");

                    b.Property<string>("FileUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("UploaderID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.HasIndex("BaseVersionID");

                    b.HasIndex("FileID");

                    b.HasIndex("UploaderID");

                    b.ToTable("FileVersion");
                });

            modelBuilder.Entity("Content.Core.Entities.File", b =>
                {
                    b.HasOne("Content.Core.Entities.Account", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Content.Core.Entities.FileVersion", b =>
                {
                    b.HasOne("Content.Core.Entities.FileVersion", "BaseVersion")
                        .WithMany()
                        .HasForeignKey("BaseVersionID");

                    b.HasOne("Content.Core.Entities.File", "File")
                        .WithMany("Versions")
                        .HasForeignKey("FileID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Content.Core.Entities.Account", "Uploader")
                        .WithMany()
                        .HasForeignKey("UploaderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaseVersion");

                    b.Navigation("File");

                    b.Navigation("Uploader");
                });

            modelBuilder.Entity("Content.Core.Entities.File", b =>
                {
                    b.Navigation("Versions");
                });
#pragma warning restore 612, 618
        }
    }
}
