using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LBGDBMetadata.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    DatabaseID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    WikipediaURL = table.Column<string>(nullable: true),
                    Platform = table.Column<string>(nullable: true),
                    ESRB = table.Column<string>(nullable: true),
                    CommunityRatingCount = table.Column<long>(nullable: false),
                    Genres = table.Column<string>(nullable: true),
                    Developer = table.Column<string>(nullable: true),
                    Publisher = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: true),
                    CommunityRating = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.DatabaseID);
                });

            migrationBuilder.CreateTable(
                name: "GameAlternateName",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DatabaseID = table.Column<long>(nullable: false),
                    AlternateName = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameAlternateName", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GameAlternateName_Games_DatabaseID",
                        column: x => x.DatabaseID,
                        principalTable: "Games",
                        principalColumn: "DatabaseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameImages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DatabaseID = table.Column<long>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GameImages_Games_DatabaseID",
                        column: x => x.DatabaseID,
                        principalTable: "Games",
                        principalColumn: "DatabaseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameAlternateName_DatabaseID",
                table: "GameAlternateName",
                column: "DatabaseID");

            migrationBuilder.CreateIndex(
                name: "IX_GameImages_DatabaseID",
                table: "GameImages",
                column: "DatabaseID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameAlternateName");

            migrationBuilder.DropTable(
                name: "GameImages");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
