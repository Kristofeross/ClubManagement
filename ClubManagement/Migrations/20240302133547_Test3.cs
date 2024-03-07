using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubManagement.Migrations
{
    public partial class Test3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WhichFoot",
                table: "Footballers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "Footballers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Footballers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Coaches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoachId",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FootballerId",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Footballers_AccountId",
                table: "Footballers",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_AccountId",
                table: "Coaches",
                column: "AccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_Accounts_AccountId",
                table: "Coaches",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Footballers_Accounts_AccountId",
                table: "Footballers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_Accounts_AccountId",
                table: "Coaches");

            migrationBuilder.DropForeignKey(
                name: "FK_Footballers_Accounts_AccountId",
                table: "Footballers");

            migrationBuilder.DropIndex(
                name: "IX_Footballers_AccountId",
                table: "Footballers");

            migrationBuilder.DropIndex(
                name: "IX_Coaches_AccountId",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Footballers");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "FootballerId",
                table: "Accounts");

            migrationBuilder.AlterColumn<int>(
                name: "WhichFoot",
                table: "Footballers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "Footballers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
