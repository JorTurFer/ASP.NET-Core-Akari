using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class linea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacturasLineas_Referencias_IdReferencia",
                schema: "Facturas",
                table: "FacturasLineas");

            migrationBuilder.DropIndex(
                name: "IX_FacturasLineas_IdReferencia",
                schema: "Facturas",
                table: "FacturasLineas");

            migrationBuilder.DropColumn(
                name: "IdReferencia",
                schema: "Facturas",
                table: "FacturasLineas");

            migrationBuilder.AddColumn<string>(
                name: "Concepto",
                schema: "Facturas",
                table: "FacturasLineas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Precio",
                schema: "Facturas",
                table: "FacturasLineas",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Concepto",
                schema: "Facturas",
                table: "FacturasLineas");

            migrationBuilder.DropColumn(
                name: "Precio",
                schema: "Facturas",
                table: "FacturasLineas");

            migrationBuilder.AddColumn<int>(
                name: "IdReferencia",
                schema: "Facturas",
                table: "FacturasLineas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FacturasLineas_IdReferencia",
                schema: "Facturas",
                table: "FacturasLineas",
                column: "IdReferencia");

            migrationBuilder.AddForeignKey(
                name: "FK_FacturasLineas_Referencias_IdReferencia",
                schema: "Facturas",
                table: "FacturasLineas",
                column: "IdReferencia",
                principalSchema: "Facturas",
                principalTable: "Referencias",
                principalColumn: "IdReferencia",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
