using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubManagement.Migrations
{
    public partial class Test6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Footballers_Accounts_AccountId",
                table: "Footballers");

            migrationBuilder.DropForeignKey(
                name: "FK_Footballers_Statistics_StatisticsId",
                table: "Footballers");

            migrationBuilder.DropIndex(
                name: "IX_Footballers_StatisticsId",
                table: "Footballers");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Accounts",
                newName: "AccountName");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_FootballerId",
                table: "Statistics",
                column: "FootballerId",
                unique: true,
                filter: "[FootballerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Footballers_Accounts_AccountId",
                table: "Footballers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Footballers_FootballerId",
                table: "Statistics",
                column: "FootballerId",
                principalTable: "Footballers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Footballers_Accounts_AccountId",
                table: "Footballers");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_Footballers_FootballerId",
                table: "Statistics");

            migrationBuilder.DropIndex(
                name: "IX_Statistics_FootballerId",
                table: "Statistics");

            migrationBuilder.RenameColumn(
                name: "AccountName",
                table: "Accounts",
                newName: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Footballers_StatisticsId",
                table: "Footballers",
                column: "StatisticsId",
                unique: true,
                filter: "[StatisticsId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Footballers_Accounts_AccountId",
                table: "Footballers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Footballers_Statistics_StatisticsId",
                table: "Footballers",
                column: "StatisticsId",
                principalTable: "Statistics",
                principalColumn: "Id");
        }
    }
}
