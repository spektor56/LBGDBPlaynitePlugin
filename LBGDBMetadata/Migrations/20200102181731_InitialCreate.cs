using Microsoft.EntityFrameworkCore.Migrations;

namespace LBGDBMetadata.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    DatabaseID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ReleaseYear = table.Column<string>(nullable: true),
                    Overview = table.Column<string>(nullable: true),
                    MaxPlayers = table.Column<string>(nullable: true),
                    Cooperative = table.Column<string>(nullable: true),
                    WikipediaURL = table.Column<string>(nullable: true),
                    Platform = table.Column<string>(nullable: true),
                    ESRB = table.Column<string>(nullable: true),
                    CommunityRatingCount = table.Column<string>(nullable: true),
                    Genres = table.Column<string>(nullable: true),
                    Developer = table.Column<string>(nullable: true),
                    Publisher = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<string>(nullable: true),
                    CommunityRating = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.DatabaseID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
