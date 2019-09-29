using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Patients");

            migrationBuilder.CreateTable(
                name: "Paises",
                schema: "Patients",
                columns: table => new
                {
                    IdPais = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.IdPais);
                });

            migrationBuilder.CreateTable(
                name: "Provincias",
                schema: "Patients",
                columns: table => new
                {
                    IdProvincia = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provincias", x => x.IdProvincia);
                });

            migrationBuilder.CreateTable(
                name: "TipoCitas",
                schema: "Patients",
                columns: table => new
                {
                    IdTipoCita = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Tipo = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCitas", x => x.IdTipoCita);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                schema: "Patients",
                columns: table => new
                {
                    IdPaciente = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: false),
                    Nacimiento = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Telefono1 = table.Column<string>(nullable: true),
                    Telefono2 = table.Column<string>(nullable: true),
                    DNI = table.Column<string>(nullable: true),
                    Direccion = table.Column<string>(nullable: true),
                    CP = table.Column<int>(nullable: false, defaultValue: 0),
                    IdProvincia = table.Column<int>(nullable: false, defaultValue: 1),
                    IdPais = table.Column<int>(nullable: false, defaultValue: 1),
                    RGPD = table.Column<bool>(nullable: false, defaultValue: false),
                    Enfermedades = table.Column<string>(nullable: true),
                    Medicación = table.Column<string>(nullable: true),
                    Alergias = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.IdPaciente);
                    table.ForeignKey(
                        name: "FK_Pacientes_Paises_IdPais",
                        column: x => x.IdPais,
                        principalSchema: "Patients",
                        principalTable: "Paises",
                        principalColumn: "IdPais",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pacientes_Provincias_IdProvincia",
                        column: x => x.IdProvincia,
                        principalSchema: "Patients",
                        principalTable: "Provincias",
                        principalColumn: "IdProvincia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalendarEvents",
                schema: "Patients",
                columns: table => new
                {
                    EventID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Subject = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    IsFullDay = table.Column<bool>(nullable: false),
                    IdPaciente = table.Column<int>(nullable: true),
                    IdTipoCita = table.Column<int>(nullable: true),
                    IsSMSSended = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEvents", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_CalendarEvents_Pacientes_IdPaciente",
                        column: x => x.IdPaciente,
                        principalSchema: "Patients",
                        principalTable: "Pacientes",
                        principalColumn: "IdPaciente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalendarEvents_TipoCitas_IdTipoCita",
                        column: x => x.IdTipoCita,
                        principalSchema: "Patients",
                        principalTable: "TipoCitas",
                        principalColumn: "IdTipoCita",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_IdPaciente",
                schema: "Patients",
                table: "CalendarEvents",
                column: "IdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_IdTipoCita",
                schema: "Patients",
                table: "CalendarEvents",
                column: "IdTipoCita");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_IdPais",
                schema: "Patients",
                table: "Pacientes",
                column: "IdPais");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_IdProvincia",
                schema: "Patients",
                table: "Pacientes",
                column: "IdProvincia");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarEvents",
                schema: "Patients");

            migrationBuilder.DropTable(
                name: "Pacientes",
                schema: "Patients");

            migrationBuilder.DropTable(
                name: "TipoCitas",
                schema: "Patients");

            migrationBuilder.DropTable(
                name: "Paises",
                schema: "Patients");

            migrationBuilder.DropTable(
                name: "Provincias",
                schema: "Patients");
        }
    }
}
