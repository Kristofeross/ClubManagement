using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubManagement.Migrations
{
    public partial class Test7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_Footballers_FootballerId",
                table: "Statistics");

            migrationBuilder.DropIndex(
                name: "IX_Statistics_FootballerId",
                table: "Statistics");

            migrationBuilder.CreateIndex(
                name: "IX_Footballers_StatisticsId",
                table: "Footballers",
                column: "StatisticsId",
                unique: true,
                filter: "[StatisticsId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Footballers_Statistics_StatisticsId",
                table: "Footballers",
                column: "StatisticsId",
                principalTable: "Statistics",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Footballers_Statistics_StatisticsId",
                table: "Footballers");

            migrationBuilder.DropIndex(
                name: "IX_Footballers_StatisticsId",
                table: "Footballers");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_FootballerId",
                table: "Statistics",
                column: "FootballerId",
                unique: true,
                filter: "[FootballerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Footballers_FootballerId",
                table: "Statistics",
                column: "FootballerId",
                principalTable: "Footballers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
