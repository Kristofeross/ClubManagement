using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubManagement.Migrations
{
    public partial class T11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdForSubstitute",
                table: "SubstituteMatchPlayers",
                newName: "FootballerId");

            migrationBuilder.RenameColumn(
                name: "IdForEleven",
                table: "PrimaryMatchPlayers",
                newName: "FootballerId");

            migrationBuilder.CreateIndex(
                name: "IX_SubstituteMatchPlayers_FootballerId",
                table: "SubstituteMatchPlayers",
                column: "FootballerId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryMatchPlayers_FootballerId",
                table: "PrimaryMatchPlayers",
                column: "FootballerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryMatchPlayers_Footballers_FootballerId",
                table: "PrimaryMatchPlayers",
                column: "FootballerId",
                principalTable: "Footballers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubstituteMatchPlayers_Footballers_FootballerId",
                table: "SubstituteMatchPlayers",
                column: "FootballerId",
                principalTable: "Footballers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryMatchPlayers_Footballers_FootballerId",
                table: "PrimaryMatchPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_SubstituteMatchPlayers_Footballers_FootballerId",
                table: "SubstituteMatchPlayers");

            migrationBuilder.DropIndex(
                name: "IX_SubstituteMatchPlayers_FootballerId",
                table: "SubstituteMatchPlayers");

            migrationBuilder.DropIndex(
                name: "IX_PrimaryMatchPlayers_FootballerId",
                table: "PrimaryMatchPlayers");

            migrationBuilder.RenameColumn(
                name: "FootballerId",
                table: "SubstituteMatchPlayers",
                newName: "IdForSubstitute");

            migrationBuilder.RenameColumn(
                name: "FootballerId",
                table: "PrimaryMatchPlayers",
                newName: "IdForEleven");
        }
    }
}
