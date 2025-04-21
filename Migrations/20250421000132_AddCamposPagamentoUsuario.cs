using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscolaDanca.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposPagamentoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataAssinatura",
                table: "Usuarios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusPagamento",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidadeAssinatura",
                table: "Usuarios",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataAssinatura",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "StatusPagamento",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ValidadeAssinatura",
                table: "Usuarios");
        }
    }
}
