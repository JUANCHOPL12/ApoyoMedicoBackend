using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApoyoClinicoCol.Api.Models
{
    [Table("Presentaciones")]
    public class Presentacion
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("medicamento_id")]
        public int MedicamentoId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("tipo_presentacion")]
        public string TipoPresentacion { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [Column("descripcion_comercial")]
        public string DescripcionComercial { get; set; } = string.Empty;

        [Column("cantidad_principio_mg", TypeName = "decimal(10,4)")]
        public decimal CantidadPrincipioMg { get; set; }

        [Column("cantidad_vehiculo", TypeName = "decimal(10,4)")]
        public decimal CantidadVehiculo { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("unidad_vehiculo")]
        public string UnidadVehiculo { get; set; } = "ml";

        [Column("activo")]
        public bool Activo { get; set; } = true;

        // Relación hacia la tabla padre Medicamentos
        [ForeignKey(nameof(MedicamentoId))]
        public Medicamento? Medicamento { get; set; }
    }
}