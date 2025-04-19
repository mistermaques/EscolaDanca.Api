using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscolaDanca.Api.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaAulaHorarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaSemana",
                table: "Aulas");

            migrationBuilder.DropColumn(
                name: "Horario",
                table: "Aulas");

            migrationBuilder.AddColumn<int>(
                name: "ProfessorId",
                table: "Aulas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HorariosAulas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiaSemana = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Horario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AulaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosAulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorariosAulas_Aulas_AulaId",
                        column: x => x.AulaId,
                        principalTable: "Aulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HorariosAulas_AulaId",
                table: "HorariosAulas",
                column: "AulaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HorariosAulas");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "Aulas");

            migrationBuilder.AddColumn<string>(
                name: "DiaSemana",
                table: "Aulas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Horario",
                table: "Aulas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
