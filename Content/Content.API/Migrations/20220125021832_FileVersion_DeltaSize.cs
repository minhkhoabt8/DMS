using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.API.Migrations
{
    public partial class FileVersion_DeltaSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DeltaSize",
                table: "FileVersions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeltaSize",
                table: "FileVersions");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Accounts");
        }
    }
}
