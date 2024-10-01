using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comtec.AS400XML.Migrations
{
    /// <inheritdoc />
    public partial class MergeColumnsOfXmlFileToXmlFilePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourcePath",
                table: "XmlFiles");

            migrationBuilder.DropColumn(
                name: "XmlFileDirectory",
                table: "XmlFiles");

            migrationBuilder.RenameColumn(
                name: "XmlFileName",
                table: "XmlFiles",
                newName: "XmlFilePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "XmlFilePath",
                table: "XmlFiles",
                newName: "XmlFileName");

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
        }
    }
}
