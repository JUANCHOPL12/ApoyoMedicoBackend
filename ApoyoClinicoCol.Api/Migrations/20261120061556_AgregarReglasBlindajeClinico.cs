using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApoyoClinicoCol.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarReglasBlindajeClinico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presentaciones_Medicamentos_medicamento_id",
                table: "Presentaciones");

            migrationBuilder.AddColumn<int>(
                name: "validado_por",
                table: "Esquemas_Dosis",
                type: "int",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presentaciones_Positivos",
                table: "Presentaciones",
                sql: "[cantidad_principio_mg] > 0 AND [cantidad_vehiculo] > 0");

            migrationBuilder.CreateIndex(
                name: "IX_Esquemas_Dosis_validado_por",
                table: "Esquemas_Dosis",
                column: "validado_por");

            migrationBuilder.AddCheckConstraint(
                name: "CK_EsquemasDosis_Positivos",
                table: "Esquemas_Dosis",
                sql: "[dosis_habitual_mg_kg] > 0 AND [frecuencia_horas_defecto] > 0 AND ([dosis_minima_mg_kg] IS NULL OR [dosis_minima_mg_kg] > 0) AND ([dosis_maxima_mg_kg] IS NULL OR [dosis_maxima_mg_kg] > 0) AND ([tope_maximo_dosis_mg] IS NULL OR [tope_maximo_dosis_mg] > 0) AND ([tope_maximo_diario_mg] IS NULL OR [tope_maximo_diario_mg] > 0)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_EsquemasDosis_Rangos",
                table: "Esquemas_Dosis",
                sql: "[edad_minima_dias] >= 0 AND ([edad_maxima_dias] IS NULL OR [edad_minima_dias] <= [edad_maxima_dias]) AND ([peso_minimo_kg] IS NULL OR [peso_minimo_kg] > 0) AND ([peso_maximo_kg] IS NULL OR [peso_minimo_kg] IS NULL OR [peso_minimo_kg] <= [peso_maximo_kg])");

            migrationBuilder.AddCheckConstraint(
                name: "CK_EsquemasDosis_ValidadoCompleto",
                table: "Esquemas_Dosis",
                sql: "[estado] <> 'Validado' OR ([fecha_revision] IS NOT NULL AND [validado_por] IS NOT NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_Esquemas_Dosis_Usuarios_validado_por",
                table: "Esquemas_Dosis",
                column: "validado_por",
                principalTable: "Usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Presentaciones_Medicamentos_medicamento_id",
                table: "Presentaciones",
                column: "medicamento_id",
                principalTable: "Medicamentos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Esquemas_Dosis_Usuarios_validado_por",
                table: "Esquemas_Dosis");

            migrationBuilder.DropForeignKey(
                name: "FK_Presentaciones_Medicamentos_medicamento_id",
                table: "Presentaciones");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presentaciones_Positivos",
                table: "Presentaciones");

            migrationBuilder.DropIndex(
                name: "IX_Esquemas_Dosis_validado_por",
                table: "Esquemas_Dosis");

            migrationBuilder.DropCheckConstraint(
                name: "CK_EsquemasDosis_Positivos",
                table: "Esquemas_Dosis");

            migrationBuilder.DropCheckConstraint(
                name: "CK_EsquemasDosis_Rangos",
                table: "Esquemas_Dosis");

            migrationBuilder.DropCheckConstraint(
                name: "CK_EsquemasDosis_ValidadoCompleto",
                table: "Esquemas_Dosis");

            migrationBuilder.DropColumn(
                name: "validado_por",
                table: "Esquemas_Dosis");

            migrationBuilder.AddForeignKey(
                name: "FK_Presentaciones_Medicamentos_medicamento_id",
                table: "Presentaciones",
                column: "medicamento_id",
                principalTable: "Medicamentos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
