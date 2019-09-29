using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class facturas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Facturas");

            migrationBuilder.CreateTable(
                name: "FacturasHeaders",
                schema: "Facturas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdPaciente = table.Column<int>(nullable: false),
                    Codigo = table.Column<string>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    IRPF = table.Column<double>(nullable: false),
                    Descuento = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturasHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacturasHeaders_Pacientes_IdPaciente",
                        column: x => x.IdPaciente,
                        principalSchema: "Patients",
                        principalTable: "Pacientes",
                        principalColumn: "IdPaciente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Referencias",
                schema: "Facturas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Identificador = table.Column<string>(nullable: false),
                    Concepto = table.Column<string>(nullable: false),
                    Precio = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referencias", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacturasHeaders_IdPaciente",
                schema: "Facturas",
                table: "FacturasHeaders",
                column: "IdPaciente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacturasHeaders",
                schema: "Facturas");

            migrationBuilder.DropTable(
                name: "Referencias",
                schema: "Facturas");
        }
    }
}
