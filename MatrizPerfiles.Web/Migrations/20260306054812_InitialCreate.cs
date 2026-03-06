using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MatrizPerfiles.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Puestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puestos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sistemas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sistemas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PbiMatrizPerfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcionUnidadOrganizativa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SistemaId = table.Column<int>(type: "int", nullable: true),
                    PuestoId = table.Column<int>(type: "int", nullable: true),
                    Modulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubModulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Menu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sitio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubSitio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpcionDeMenu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Opcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Programa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArchivoDePrograma = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionRolAsociado = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CodigoIdFuncion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Operacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vistas = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nivel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Carpeta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuenteDeDatos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grupo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trans = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bloque = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoDePerfil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Workflow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Workview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubMenu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionNivel1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionNivel2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionNivel3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionNivel4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionNivel5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionNivel6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionNivel7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbiMatrizPerfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PbiMatrizPerfiles_Puestos_PuestoId",
                        column: x => x.PuestoId,
                        principalTable: "Puestos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PbiMatrizPerfiles_Sistemas_SistemaId",
                        column: x => x.SistemaId,
                        principalTable: "Sistemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Puestos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Analista de Crédito" },
                    { 2, "Gerente de Sucursal" },
                    { 3, "Cajero" },
                    { 4, "Auditor Interno" }
                });

            migrationBuilder.InsertData(
                table: "Sistemas",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Sistema Core" },
                    { 2, "CRM" },
                    { 3, "ERP Financiero" },
                    { 4, "Banca Móvil" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbiMatrizPerfil_Codigo",
                table: "PbiMatrizPerfiles",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_PbiMatrizPerfil_FuncionRolAsociado",
                table: "PbiMatrizPerfiles",
                column: "FuncionRolAsociado");

            migrationBuilder.CreateIndex(
                name: "IX_PbiMatrizPerfil_SistemaId",
                table: "PbiMatrizPerfiles",
                column: "SistemaId");

            migrationBuilder.CreateIndex(
                name: "IX_PbiMatrizPerfil_Vistas",
                table: "PbiMatrizPerfiles",
                column: "Vistas");

            migrationBuilder.CreateIndex(
                name: "IX_PbiMatrizPerfiles_PuestoId",
                table: "PbiMatrizPerfiles",
                column: "PuestoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PbiMatrizPerfiles");

            migrationBuilder.DropTable(
                name: "Puestos");

            migrationBuilder.DropTable(
                name: "Sistemas");
        }
    }
}
