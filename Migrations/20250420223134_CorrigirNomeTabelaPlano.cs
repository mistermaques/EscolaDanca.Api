using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscolaDanca.Api.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirNomeTabelaPlano : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Plano_PlanoId",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plano",
                table: "Plano");

            migrationBuilder.RenameTable(
                name: "Plano",
                newName: "Planos");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "Planos",
                newName: "ValorMensal");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Planos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Planos",
                table: "Planos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PlanosAulas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanoId = table.Column<int>(type: "int", nullable: false),
                    AulaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanosAulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanosAulas_Aulas_AulaId",
                        column: x => x.AulaId,
                        principalTable: "Aulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanosAulas_Planos_PlanoId",
                        column: x => x.PlanoId,
                        principalTable: "Planos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanosAulas_AulaId",
                table: "PlanosAulas",
                column: "AulaId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanosAulas_PlanoId",
                table: "PlanosAulas",
                column: "PlanoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Planos_PlanoId",
                table: "Usuarios",
                column: "PlanoId",
                principalTable: "Planos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Planos_PlanoId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "PlanosAulas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Planos",
                table: "Planos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Planos");

            migrationBuilder.RenameTable(
                name: "Planos",
                newName: "Plano");

            migrationBuilder.RenameColumn(
                name: "ValorMensal",
                table: "Plano",
                newName: "Valor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plano",
                table: "Plano",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Plano_PlanoId",
                table: "Usuarios",
                column: "PlanoId",
                principalTable: "Plano",
                principalColumn: "Id");
        }
    }
}
