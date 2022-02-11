using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.API.Migrations
{
    public partial class File_Folder_Owner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerID",
                table: "Folders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerID",
                table: "Files",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Folders_OwnerID",
                table: "Folders",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Files_OwnerID",
                table: "Files",
                column: "OwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Accounts_OwnerID",
                table: "Files",
                column: "OwnerID",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Accounts_OwnerID",
                table: "Folders",
                column: "OwnerID",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Accounts_OwnerID",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Accounts_OwnerID",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_OwnerID",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Files_OwnerID",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Files");
        }
    }
}
