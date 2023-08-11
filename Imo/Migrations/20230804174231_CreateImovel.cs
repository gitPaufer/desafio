using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imo.Migrations
{
    /// <inheritdoc />
    public partial class CreateImovel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Imovel_Fotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdImovel = table.Column<int>(type: "int", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescricaoFoto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PathFoto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    IdUser = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imovel_Fotos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Imovels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Localizacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactoFone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactoEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoNegocio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PathFotoPrincipal = table.Column<string>(type: "nvarchar(max)", nullable: false),

                    Date = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    IdUser = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imovels", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Imovel_Fotos");

            migrationBuilder.DropTable(
                name: "Imovels");
        }
    }
}
