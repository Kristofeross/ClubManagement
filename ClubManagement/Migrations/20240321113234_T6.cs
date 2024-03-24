using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubManagement.Migrations
{
    public partial class T6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Matches",
                newName: "StartMatch");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Matches",
                newName: "DateOfMatch");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartMatch",
                table: "Matches",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "DateOfMatch",
                table: "Matches",
                newName: "Date");
        }
    }
}
