using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubManagement.Migrations
{
    public partial class T4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoachMatch");

            migrationBuilder.DropColumn(
                name: "IsCancelledOrPostponed",
                table: "Matches");

            migrationBuilder.AddColumn<string>(
                name: "AgeCategory",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MainCoachId",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MatchStatus",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypeOfMatch",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_MainCoachId",
                table: "Matches",
                column: "MainCoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Coaches_MainCoachId",
                table: "Matches",
                column: "MainCoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Coaches_MainCoachId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_MainCoachId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "AgeCategory",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MainCoachId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchStatus",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TypeOfMatch",
                table: "Matches");

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelledOrPostponed",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CoachMatch",
                columns: table => new
                {
                    CoachesId = table.Column<int>(type: "int", nullable: false),
                    MatchesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachMatch", x => new { x.CoachesId, x.MatchesId });
                    table.ForeignKey(
                        name: "FK_CoachMatch_Coaches_CoachesId",
                        column: x => x.CoachesId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoachMatch_Matches_MatchesId",
                        column: x => x.MatchesId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoachMatch_MatchesId",
                table: "CoachMatch",
                column: "MatchesId");
        }
    }
}
