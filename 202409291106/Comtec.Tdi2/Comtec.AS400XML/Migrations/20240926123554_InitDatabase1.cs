using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comtec.AS400XML.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourcePath",
                table: "XmlFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "XmlFileDirectory",
                table: "XmlFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "XmlFileName",
                table: "XmlFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourcePath",
                table: "XmlFiles");

            migrationBuilder.DropColumn(
                name: "XmlFileDirectory",
                table: "XmlFiles");

            migrationBuilder.DropColumn(
                name: "XmlFileName",
                table: "XmlFiles");
        }
    }
}
