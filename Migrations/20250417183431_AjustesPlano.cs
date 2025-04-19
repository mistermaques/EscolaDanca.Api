using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscolaDanca.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjustesPlano : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanoId",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Plano",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plano", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PlanoId",
                table: "Usuarios",
                column: "PlanoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Plano_PlanoId",
                table: "Usuarios",
                column: "PlanoId",
                principalTable: "Plano",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Plano_PlanoId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Plano");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_PlanoId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PlanoId",
                table: "Usuarios");
        }
    }
}
