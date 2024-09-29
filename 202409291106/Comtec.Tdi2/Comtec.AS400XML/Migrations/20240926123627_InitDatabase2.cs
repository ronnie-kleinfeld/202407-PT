using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comtec.AS400XML.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ZXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "YZonesXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "YZonesBkgXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "YXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "XZonesXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "XZonesBkgXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "XXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "XmlFiles");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "SXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ScreenXmlRoot");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "RXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "RctXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "RctBkgXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "LXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "FXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "FolderXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "FolderDetailsXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "FieldsXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "DXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "CXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "CmdXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "BXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "BgrXmlElement");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ArXmlElement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ZXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "YZonesXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "YZonesBkgXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "YXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "XZonesXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "XZonesBkgXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "XXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "XmlFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "SXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ScreenXmlRoot",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "RXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "RctXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "RctBkgXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "LXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "FXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "FolderXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "FolderDetailsXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "FieldsXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "DXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CmdXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "BXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "BgrXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ArXmlElement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
