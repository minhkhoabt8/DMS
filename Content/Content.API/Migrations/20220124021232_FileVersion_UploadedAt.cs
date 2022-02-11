using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.API.Migrations
{
    public partial class FileVersion_UploadedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "FileVersion",
                newName: "UploadedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UploadedAt",
                table: "FileVersion",
                newName: "CreatedAt");
        }
    }
}
