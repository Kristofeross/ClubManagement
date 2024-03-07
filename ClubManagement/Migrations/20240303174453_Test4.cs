using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubManagement.Migrations
{
    public partial class Test4 : Migration
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
                name: "IX_Footballers_AccountId",
                table: "Footballers");

            migrationBuilder.DropIndex(
                name: "IX_Footballers_StatisticsId",
                table: "Footballers");

            migrationBuilder.AlterColumn<int>(
                name: "FootballerId",
                table: "Statistics",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "StatisticsId",
                table: "Footballers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Footballers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Footballers_AccountId",
                table: "Footballers",
                column: "AccountId",
                unique: true,
                filter: "[AccountId] IS NOT NULL");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Footballers_Accounts_AccountId",
                table: "Footballers");

            migrationBuilder.DropForeignKey(
                name: "FK_Footballers_Statistics_StatisticsId",
                table: "Footballers");

            migrationBuilder.DropIndex(
                name: "IX_Footballers_AccountId",
                table: "Footballers");

            migrationBuilder.DropIndex(
                name: "IX_Footballers_StatisticsId",
                table: "Footballers");

            migrationBuilder.AlterColumn<int>(
                name: "FootballerId",
                table: "Statistics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatisticsId",
                table: "Footballers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Footballers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Footballers_AccountId",
                table: "Footballers",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Footballers_StatisticsId",
                table: "Footballers",
                column: "StatisticsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Footballers_Accounts_AccountId",
                table: "Footballers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Footballers_Statistics_StatisticsId",
                table: "Footballers",
                column: "StatisticsId",
                principalTable: "Statistics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
