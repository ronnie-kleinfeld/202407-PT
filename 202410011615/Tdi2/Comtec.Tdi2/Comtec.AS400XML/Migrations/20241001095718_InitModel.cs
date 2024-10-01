using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comtec.AS400XML.Migrations
{
    /// <inheritdoc />
    public partial class InitModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BgrXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BgrXmlElement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CmdXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmdXmlElement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldsXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldsXmlElement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FolderDetailsXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderDetailsXmlElement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FolderXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderXmlElement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RctBkgXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RctBkgXmlElement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RctXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RctXmlElement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdrXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ArwXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CliXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DtkXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DtrXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DspaXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FchXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FcmdXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FdspXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FgrXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FilXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FindXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FlangXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FldXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FlibXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FlpXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FlrXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FnnXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FortXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FpcXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FpsXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FsidXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GrkXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    JacketXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MsgXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PicXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PlgcXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PntXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PsetXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PxrXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    QendXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    QflxXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    QpxlXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    QstrXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RecXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SpoXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SrgXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StrcXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    VerXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WaitXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WcolXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WcstXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WinXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WlinXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WlstXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WsmlXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SXmlElement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XZonesBkgXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XZonesBkgXmlElement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XZonesXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XZonesXmlElement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YZonesBkgXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YZonesBkgXmlElement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YZonesXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YZonesXmlElement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZclXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ZlnXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BgrXmlElementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZXmlElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZXmlElement_BgrXmlElement_BgrXmlElementId",
                        column: x => x.BgrXmlElementId,
                        principalTable: "BgrXmlElement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CgrXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FbuaXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FkXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FtXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LenXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PbgXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PicXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PntXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PszXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CmdXmlElementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CXmlElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CXmlElement_CmdXmlElement_CmdXmlElementId",
                        column: x => x.CmdXmlElementId,
                        principalTable: "CmdXmlElement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AliasXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AprXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BkgXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BuaXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ChbXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ColXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CryXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DecXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DfsXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EdtXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EmlXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EwrXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    F4PXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HebXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HkyXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IndXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    InpXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LenXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LinXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LnkXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LvlXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ManXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MaxXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MinXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NumXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NumvXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OnlyXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PbgXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PchXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PchlXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PclXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PerXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Pf4XmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PfnXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PhiXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PhtXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PicXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PkvXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PlcXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PntXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PrlXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PslXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PszXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PtpXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PulXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PwdXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PxkXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PxmXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PxnXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PxrXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    QfkXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RtpXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SgnXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TabXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TchXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TipXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TypXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ValXmlAttribute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WdXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FieldsXmlElementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FXmlElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FXmlElement_FieldsXmlElement_FieldsXmlElementId",
                        column: x => x.FieldsXmlElementId,
                        principalTable: "FieldsXmlElement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QtypXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FolderDetailsXmlElementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DXmlElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DXmlElement_FolderDetailsXmlElement_FolderDetailsXmlElementId",
                        column: x => x.FolderDetailsXmlElementId,
                        principalTable: "FolderDetailsXmlElement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FclXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IcnXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LkXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LnXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LonXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LtrXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FolderXmlElementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LXmlElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LXmlElement_FolderXmlElement_FolderXmlElementId",
                        column: x => x.FolderXmlElementId,
                        principalTable: "FolderXmlElement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FicnXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FtitXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RclXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Rx1XmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Rx2XmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Ry1XmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Ry2XmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RctXmlElementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArXmlElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArXmlElement_RctXmlElement_RctXmlElementId",
                        column: x => x.RctXmlElementId,
                        principalTable: "RctXmlElement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BkgXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FicnXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FtitXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RclXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Rx1XmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Rx2XmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Ry1XmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Ry2XmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RctBkgXmlElementId = table.Column<int>(type: "int", nullable: true),
                    RctXmlElementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RXmlElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RXmlElement_RctBkgXmlElement_RctBkgXmlElementId",
                        column: x => x.RctBkgXmlElementId,
                        principalTable: "RctBkgXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RXmlElement_RctXmlElement_RctXmlElementId",
                        column: x => x.RctXmlElementId,
                        principalTable: "RctXmlElement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "XXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BkgXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    XnXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    XsXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    XZonesBkgXmlElementId = table.Column<int>(type: "int", nullable: true),
                    XZonesXmlElementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XXmlElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XXmlElement_XZonesBkgXmlElement_XZonesBkgXmlElementId",
                        column: x => x.XZonesBkgXmlElementId,
                        principalTable: "XZonesBkgXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_XXmlElement_XZonesXmlElement_XZonesXmlElementId",
                        column: x => x.XZonesXmlElementId,
                        principalTable: "XZonesXmlElement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScreenOutputXmlRoot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BgrXmlElementId = table.Column<int>(type: "int", nullable: true),
                    CmdXmlElementId = table.Column<int>(type: "int", nullable: true),
                    FieldsXmlElementId = table.Column<int>(type: "int", nullable: true),
                    FolderXmlElementId = table.Column<int>(type: "int", nullable: true),
                    FolderDetailsXmlElementId = table.Column<int>(type: "int", nullable: true),
                    RctXmlElementId = table.Column<int>(type: "int", nullable: true),
                    RctBkgXmlElementId = table.Column<int>(type: "int", nullable: true),
                    SXmlElementId = table.Column<int>(type: "int", nullable: true),
                    XZonesXmlElementId = table.Column<int>(type: "int", nullable: true),
                    XZonesBkgXmlElementId = table.Column<int>(type: "int", nullable: true),
                    YZonesXmlElementId = table.Column<int>(type: "int", nullable: true),
                    YZonesBkgXmlElementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenOutputXmlRoot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreenOutputXmlRoot_BgrXmlElement_BgrXmlElementId",
                        column: x => x.BgrXmlElementId,
                        principalTable: "BgrXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScreenOutputXmlRoot_CmdXmlElement_CmdXmlElementId",
                        column: x => x.CmdXmlElementId,
                        principalTable: "CmdXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScreenOutputXmlRoot_FieldsXmlElement_FieldsXmlElementId",
                        column: x => x.FieldsXmlElementId,
                        principalTable: "FieldsXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScreenOutputXmlRoot_FolderDetailsXmlElement_FolderDetailsXmlElementId",
                        column: x => x.FolderDetailsXmlElementId,
                        principalTable: "FolderDetailsXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScreenOutputXmlRoot_FolderXmlElement_FolderXmlElementId",
                        column: x => x.FolderXmlElementId,
                        principalTable: "FolderXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScreenOutputXmlRoot_RctBkgXmlElement_RctBkgXmlElementId",
                        column: x => x.RctBkgXmlElementId,
                        principalTable: "RctBkgXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScreenOutputXmlRoot_RctXmlElement_RctXmlElementId",
                        column: x => x.RctXmlElementId,
                        principalTable: "RctXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScreenOutputXmlRoot_SXmlElement_SXmlElementId",
                        column: x => x.SXmlElementId,
                        principalTable: "SXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScreenOutputXmlRoot_XZonesBkgXmlElement_XZonesBkgXmlElementId",
                        column: x => x.XZonesBkgXmlElementId,
                        principalTable: "XZonesBkgXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScreenOutputXmlRoot_XZonesXmlElement_XZonesXmlElementId",
                        column: x => x.XZonesXmlElementId,
                        principalTable: "XZonesXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScreenOutputXmlRoot_YZonesBkgXmlElement_YZonesBkgXmlElementId",
                        column: x => x.YZonesBkgXmlElementId,
                        principalTable: "YZonesBkgXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScreenOutputXmlRoot_YZonesXmlElement_YZonesXmlElementId",
                        column: x => x.YZonesXmlElementId,
                        principalTable: "YZonesXmlElement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "YXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BkgXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    YnXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    YsXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    YtXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    YZonesBkgXmlElementId = table.Column<int>(type: "int", nullable: true),
                    YZonesXmlElementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YXmlElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YXmlElement_YZonesBkgXmlElement_YZonesBkgXmlElementId",
                        column: x => x.YZonesBkgXmlElementId,
                        principalTable: "YZonesBkgXmlElement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_YXmlElement_YZonesXmlElement_YZonesXmlElementId",
                        column: x => x.YZonesXmlElementId,
                        principalTable: "YZonesXmlElement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BXmlElement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NmXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OpXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RoXmlAttribute = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FXmlElementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXmlElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXmlElement_FXmlElement_FXmlElementId",
                        column: x => x.FXmlElementId,
                        principalTable: "FXmlElement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "XmlFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    XmlFileDirectory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    XmlFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourcePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RawXML = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScreenXmlRootId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XmlFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XmlFiles_ScreenOutputXmlRoot_ScreenXmlRootId",
                        column: x => x.ScreenXmlRootId,
                        principalTable: "ScreenOutputXmlRoot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArXmlElement_RctXmlElementId",
                table: "ArXmlElement",
                column: "RctXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_BXmlElement_FXmlElementId",
                table: "BXmlElement",
                column: "FXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_CXmlElement_CmdXmlElementId",
                table: "CXmlElement",
                column: "CmdXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_DXmlElement_FolderDetailsXmlElementId",
                table: "DXmlElement",
                column: "FolderDetailsXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_FXmlElement_FieldsXmlElementId",
                table: "FXmlElement",
                column: "FieldsXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_LXmlElement_FolderXmlElementId",
                table: "LXmlElement",
                column: "FolderXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_RXmlElement_RctBkgXmlElementId",
                table: "RXmlElement",
                column: "RctBkgXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_RXmlElement_RctXmlElementId",
                table: "RXmlElement",
                column: "RctXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenOutputXmlRoot_BgrXmlElementId",
                table: "ScreenOutputXmlRoot",
                column: "BgrXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenOutputXmlRoot_CmdXmlElementId",
                table: "ScreenOutputXmlRoot",
                column: "CmdXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenOutputXmlRoot_FieldsXmlElementId",
                table: "ScreenOutputXmlRoot",
                column: "FieldsXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenOutputXmlRoot_FolderDetailsXmlElementId",
                table: "ScreenOutputXmlRoot",
                column: "FolderDetailsXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenOutputXmlRoot_FolderXmlElementId",
                table: "ScreenOutputXmlRoot",
                column: "FolderXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenOutputXmlRoot_RctBkgXmlElementId",
                table: "ScreenOutputXmlRoot",
                column: "RctBkgXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenOutputXmlRoot_RctXmlElementId",
                table: "ScreenOutputXmlRoot",
                column: "RctXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenOutputXmlRoot_SXmlElementId",
                table: "ScreenOutputXmlRoot",
                column: "SXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenOutputXmlRoot_XZonesBkgXmlElementId",
                table: "ScreenOutputXmlRoot",
                column: "XZonesBkgXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenOutputXmlRoot_XZonesXmlElementId",
                table: "ScreenOutputXmlRoot",
                column: "XZonesXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenOutputXmlRoot_YZonesBkgXmlElementId",
                table: "ScreenOutputXmlRoot",
                column: "YZonesBkgXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenOutputXmlRoot_YZonesXmlElementId",
                table: "ScreenOutputXmlRoot",
                column: "YZonesXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_XmlFiles_ScreenXmlRootId",
                table: "XmlFiles",
                column: "ScreenXmlRootId");

            migrationBuilder.CreateIndex(
                name: "IX_XXmlElement_XZonesBkgXmlElementId",
                table: "XXmlElement",
                column: "XZonesBkgXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_XXmlElement_XZonesXmlElementId",
                table: "XXmlElement",
                column: "XZonesXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_YXmlElement_YZonesBkgXmlElementId",
                table: "YXmlElement",
                column: "YZonesBkgXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_YXmlElement_YZonesXmlElementId",
                table: "YXmlElement",
                column: "YZonesXmlElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ZXmlElement_BgrXmlElementId",
                table: "ZXmlElement",
                column: "BgrXmlElementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArXmlElement");

            migrationBuilder.DropTable(
                name: "BXmlElement");

            migrationBuilder.DropTable(
                name: "CXmlElement");

            migrationBuilder.DropTable(
                name: "DXmlElement");

            migrationBuilder.DropTable(
                name: "LXmlElement");

            migrationBuilder.DropTable(
                name: "RXmlElement");

            migrationBuilder.DropTable(
                name: "XmlFiles");

            migrationBuilder.DropTable(
                name: "XXmlElement");

            migrationBuilder.DropTable(
                name: "YXmlElement");

            migrationBuilder.DropTable(
                name: "ZXmlElement");

            migrationBuilder.DropTable(
                name: "FXmlElement");

            migrationBuilder.DropTable(
                name: "ScreenOutputXmlRoot");

            migrationBuilder.DropTable(
                name: "BgrXmlElement");

            migrationBuilder.DropTable(
                name: "CmdXmlElement");

            migrationBuilder.DropTable(
                name: "FieldsXmlElement");

            migrationBuilder.DropTable(
                name: "FolderDetailsXmlElement");

            migrationBuilder.DropTable(
                name: "FolderXmlElement");

            migrationBuilder.DropTable(
                name: "RctBkgXmlElement");

            migrationBuilder.DropTable(
                name: "RctXmlElement");

            migrationBuilder.DropTable(
                name: "SXmlElement");

            migrationBuilder.DropTable(
                name: "XZonesBkgXmlElement");

            migrationBuilder.DropTable(
                name: "XZonesXmlElement");

            migrationBuilder.DropTable(
                name: "YZonesBkgXmlElement");

            migrationBuilder.DropTable(
                name: "YZonesXmlElement");
        }
    }
}
