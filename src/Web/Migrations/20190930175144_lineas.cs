using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class lineas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Facturas",
                table: "Referencias",
                newName: "IdReferencia");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Facturas",
                table: "FacturasHeaders",
                newName: "IdFactura");

            migrationBuilder.CreateTable(
                name: "FacturasLineas",
                schema: "Facturas",
                columns: table => new
                {
                    IdLine = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdFactura = table.Column<int>(nullable: false),
                    IdReferencia = table.Column<int>(nullable: false),
                    Cantidad = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturasLineas", x => x.IdLine);
                    table.ForeignKey(
                        name: "FK_FacturasLineas_FacturasHeaders_IdFactura",
                        column: x => x.IdFactura,
                        principalSchema: "Facturas",
                        principalTable: "FacturasHeaders",
                        principalColumn: "IdFactura",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacturasLineas_Referencias_IdReferencia",
                        column: x => x.IdReferencia,
                        principalSchema: "Facturas",
                        principalTable: "Referencias",
                        principalColumn: "IdReferencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacturasLineas_IdFactura",
                schema: "Facturas",
                table: "FacturasLineas",
                column: "IdFactura");

            migrationBuilder.CreateIndex(
                name: "IX_FacturasLineas_IdReferencia",
                schema: "Facturas",
                table: "FacturasLineas",
                column: "IdReferencia");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacturasLineas",
                schema: "Facturas");

            migrationBuilder.RenameColumn(
                name: "IdReferencia",
                schema: "Facturas",
                table: "Referencias",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdFactura",
                schema: "Facturas",
                table: "FacturasHeaders",
                newName: "Id");
        }
    }
}
