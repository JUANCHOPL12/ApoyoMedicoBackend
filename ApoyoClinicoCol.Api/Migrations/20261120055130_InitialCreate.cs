using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApoyoClinicoCol.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fuentes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    entidad = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    anio = table.Column<int>(type: "int", nullable: false),
                    enlace = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fuentes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Medicamentos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_generico = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    categoria_terapeutica = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    via_administracion = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    observacion_clinica = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicamentos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    google_sub = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    nombre_completo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    rol = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    fecha_registro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fin_prueba_gratis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estado_suscripcion = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    fin_suscripcion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id);
                    table.CheckConstraint("CK_Usuarios_EstadoSuscripcion", "[estado_suscripcion] IN ('PruebaGratis', 'Activa', 'Vencida')");
                    table.CheckConstraint("CK_Usuarios_Rol", "[rol] IN ('Medico', 'Admin')");
                });

            migrationBuilder.CreateTable(
                name: "Esquemas_Dosis",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    medicamento_id = table.Column<int>(type: "int", nullable: false),
                    fuente_id = table.Column<int>(type: "int", nullable: false),
                    nombre_esquema = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    tipo_calculo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    edad_minima_dias = table.Column<int>(type: "int", nullable: false),
                    edad_maxima_dias = table.Column<int>(type: "int", nullable: true),
                    peso_minimo_kg = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    peso_maximo_kg = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    dosis_minima_mg_kg = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    dosis_habitual_mg_kg = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    dosis_maxima_mg_kg = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    frecuencia_horas_defecto = table.Column<int>(type: "int", nullable: false),
                    frecuencias_permitidas = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    tope_maximo_dosis_mg = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    tope_maximo_diario_mg = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    fecha_revision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Esquemas_Dosis", x => x.id);
                    table.CheckConstraint("CK_EsquemasDosis_Estado", "[estado] IN ('Borrador', 'Validado')");
                    table.CheckConstraint("CK_EsquemasDosis_HabitualMenorMax", "[dosis_maxima_mg_kg] IS NULL OR [dosis_habitual_mg_kg] <= [dosis_maxima_mg_kg]");
                    table.CheckConstraint("CK_EsquemasDosis_MinMenorHabitual", "[dosis_minima_mg_kg] IS NULL OR [dosis_minima_mg_kg] <= [dosis_habitual_mg_kg]");
                    table.CheckConstraint("CK_EsquemasDosis_TipoCalculo", "[tipo_calculo] IN ('PorDia', 'PorToma')");
                    table.ForeignKey(
                        name: "FK_Esquemas_Dosis_Fuentes_fuente_id",
                        column: x => x.fuente_id,
                        principalTable: "Fuentes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Esquemas_Dosis_Medicamentos_medicamento_id",
                        column: x => x.medicamento_id,
                        principalTable: "Medicamentos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Presentaciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    medicamento_id = table.Column<int>(type: "int", nullable: false),
                    tipo_presentacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion_comercial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    cantidad_principio_mg = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    cantidad_vehiculo = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    unidad_vehiculo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presentaciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_Presentaciones_Medicamentos_medicamento_id",
                        column: x => x.medicamento_id,
                        principalTable: "Medicamentos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios_Favoritos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    medicamento_id = table.Column<int>(type: "int", nullable: false),
                    fecha_agregado = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios_Favoritos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Favoritos_Medicamentos_medicamento_id",
                        column: x => x.medicamento_id,
                        principalTable: "Medicamentos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuarios_Favoritos_Usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Esquemas_Dosis_fuente_id",
                table: "Esquemas_Dosis",
                column: "fuente_id");

            migrationBuilder.CreateIndex(
                name: "IX_Esquemas_Dosis_medicamento_id",
                table: "Esquemas_Dosis",
                column: "medicamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_nombre_generico",
                table: "Medicamentos",
                column: "nombre_generico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Presentaciones_medicamento_id",
                table: "Presentaciones",
                column: "medicamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_email",
                table: "Usuarios",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_google_sub",
                table: "Usuarios",
                column: "google_sub",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Favoritos_medicamento_id",
                table: "Usuarios_Favoritos",
                column: "medicamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Favoritos_usuario_id_medicamento_id",
                table: "Usuarios_Favoritos",
                columns: new[] { "usuario_id", "medicamento_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Esquemas_Dosis");

            migrationBuilder.DropTable(
                name: "Presentaciones");

            migrationBuilder.DropTable(
                name: "Usuarios_Favoritos");

            migrationBuilder.DropTable(
                name: "Fuentes");

            migrationBuilder.DropTable(
                name: "Medicamentos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
