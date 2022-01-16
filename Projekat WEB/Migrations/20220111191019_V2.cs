using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekat_WEB.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgencijaID",
                table: "Majstor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Agencija",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencija", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Majstor_AgencijaID",
                table: "Majstor",
                column: "AgencijaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Majstor_Agencija_AgencijaID",
                table: "Majstor",
                column: "AgencijaID",
                principalTable: "Agencija",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Majstor_Agencija_AgencijaID",
                table: "Majstor");

            migrationBuilder.DropTable(
                name: "Agencija");

            migrationBuilder.DropIndex(
                name: "IX_Majstor_AgencijaID",
                table: "Majstor");

            migrationBuilder.DropColumn(
                name: "AgencijaID",
                table: "Majstor");
        }
    }
}
