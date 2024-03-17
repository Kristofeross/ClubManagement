using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubManagement.Migrations
{
    public partial class T2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndividualTraining_Coaches_CoachId",
                table: "IndividualTraining");

            migrationBuilder.AlterColumn<int>(
                name: "CoachId",
                table: "IndividualTraining",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualTraining_Coaches_CoachId",
                table: "IndividualTraining",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndividualTraining_Coaches_CoachId",
                table: "IndividualTraining");

            migrationBuilder.AlterColumn<int>(
                name: "CoachId",
                table: "IndividualTraining",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualTraining_Coaches_CoachId",
                table: "IndividualTraining",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
