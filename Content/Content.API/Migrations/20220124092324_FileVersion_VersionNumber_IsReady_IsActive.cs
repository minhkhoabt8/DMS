using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.API.Migrations
{
    public partial class FileVersion_VersionNumber_IsReady_IsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Account_OwnerID",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_FileVersion_Account_UploaderID",
                table: "FileVersion");

            migrationBuilder.DropForeignKey(
                name: "FK_FileVersion_Files_FileID",
                table: "FileVersion");

            migrationBuilder.DropForeignKey(
                name: "FK_FileVersion_FileVersion_BaseVersionID",
                table: "FileVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileVersion",
                table: "FileVersion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "FileVersion");

            migrationBuilder.RenameTable(
                name: "FileVersion",
                newName: "FileVersions");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_FileVersion_UploaderID",
                table: "FileVersions",
                newName: "IX_FileVersions_UploaderID");

            migrationBuilder.RenameIndex(
                name: "IX_FileVersion_FileID",
                table: "FileVersions",
                newName: "IX_FileVersions_FileID");

            migrationBuilder.RenameIndex(
                name: "IX_FileVersion_BaseVersionID",
                table: "FileVersions",
                newName: "IX_FileVersions_BaseVersionID");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "FileVersions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FileVersions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "FileVersions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VersionNumber",
                table: "FileVersions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileVersions",
                table: "FileVersions",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Accounts_OwnerID",
                table: "Files",
                column: "OwnerID",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileVersions_Accounts_UploaderID",
                table: "FileVersions",
                column: "UploaderID",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileVersions_Files_FileID",
                table: "FileVersions",
                column: "FileID",
                principalTable: "Files",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileVersions_FileVersions_BaseVersionID",
                table: "FileVersions",
                column: "BaseVersionID",
                principalTable: "FileVersions",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Accounts_OwnerID",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_FileVersions_Accounts_UploaderID",
                table: "FileVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_FileVersions_Files_FileID",
                table: "FileVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_FileVersions_FileVersions_BaseVersionID",
                table: "FileVersions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileVersions",
                table: "FileVersions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FileVersions");

            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "FileVersions");

            migrationBuilder.DropColumn(
                name: "VersionNumber",
                table: "FileVersions");

            migrationBuilder.RenameTable(
                name: "FileVersions",
                newName: "FileVersion");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Account");

            migrationBuilder.RenameIndex(
                name: "IX_FileVersions_UploaderID",
                table: "FileVersion",
                newName: "IX_FileVersion_UploaderID");

            migrationBuilder.RenameIndex(
                name: "IX_FileVersions_FileID",
                table: "FileVersion",
                newName: "IX_FileVersion_FileID");

            migrationBuilder.RenameIndex(
                name: "IX_FileVersions_BaseVersionID",
                table: "FileVersion",
                newName: "IX_FileVersion_BaseVersionID");

            migrationBuilder.AlterColumn<int>(
                name: "FileName",
                table: "FileVersion",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "FileVersion",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileVersion",
                table: "FileVersion",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Account_OwnerID",
                table: "Files",
                column: "OwnerID",
                principalTable: "Account",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileVersion_Account_UploaderID",
                table: "FileVersion",
                column: "UploaderID",
                principalTable: "Account",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileVersion_Files_FileID",
                table: "FileVersion",
                column: "FileID",
                principalTable: "Files",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileVersion_FileVersion_BaseVersionID",
                table: "FileVersion",
                column: "BaseVersionID",
                principalTable: "FileVersion",
                principalColumn: "ID");
        }
    }
}
