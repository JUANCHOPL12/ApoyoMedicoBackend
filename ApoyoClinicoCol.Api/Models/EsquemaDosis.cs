using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApoyoClinicoCol.Api.Models
{
    [Table("Esquemas_Dosis")]
    public class EsquemaDosis
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("medicamento_id")]
        public int MedicamentoId { get; set; }

        [Column("fuente_id")]
        public int FuenteId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nombre_esquema")]
        public string NombreEsquema { get; set; } = "Esquema estándar";

        [Required]
        [MaxLength(20)]
        [Column("tipo_calculo")]
        public string TipoCalculo { get; set; } = "PorDia";

        [Column("edad_minima_dias")]
        public int EdadMinimaDias { get; set; } = 0;

        [Column("edad_maxima_dias")]
        public int? EdadMaximaDias { get; set; }

        [Column("peso_minimo_kg", TypeName = "decimal(6,2)")]
        public decimal? PesoMinimoKg { get; set; }

        [Column("peso_maximo_kg", TypeName = "decimal(6,2)")]
        public decimal? PesoMaximoKg { get; set; }

        [Column("dosis_minima_mg_kg", TypeName = "decimal(10,4)")]
        public decimal? DosisMinimaMgKg { get; set; }

        [Column("dosis_habitual_mg_kg", TypeName = "decimal(10,4)")]
        public decimal DosisHabitualMgKg { get; set; }

        [Column("dosis_maxima_mg_kg", TypeName = "decimal(10,4)")]
        public decimal? DosisMaximaMgKg { get; set; }

        [Column("frecuencia_horas_defecto")]
        public int FrecuenciaHorasDefecto { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("frecuencias_permitidas")]
        public string FrecuenciasPermitidas { get; set; } = string.Empty;

        [Column("tope_maximo_dosis_mg", TypeName = "decimal(10,2)")]
        public decimal? TopeMaximoDosisMg { get; set; }

        [Column("tope_maximo_diario_mg", TypeName = "decimal(10,2)")]
        public decimal? TopeMaximoDiarioMg { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("estado")]
        public string Estado { get; set; } = "Borrador";

        [Column("fecha_revision")]
        public DateTime? FechaRevision { get; set; }

        // Nuevo campo: ID del usuario (Admin/Médico) que validó clínicamente el esquema
        [Column("validado_por")]
        public int? ValidadoPorId { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        // Relaciones hacia Medicamentos, Fuentes y Usuarios
        [ForeignKey(nameof(MedicamentoId))]
        public Medicamento? Medicamento { get; set; }

        [ForeignKey(nameof(FuenteId))]
        public Fuente? Fuente { get; set; }

        [ForeignKey(nameof(ValidadoPorId))]
        public Usuario? ValidadoPor { get; set; }
    }
}