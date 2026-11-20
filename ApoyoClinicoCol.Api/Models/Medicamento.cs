using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApoyoClinicoCol.Api.Models
{
    [Table("Medicamentos")]
    public class Medicamento
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(120)]
        [Column("nombre_generico")]
        public string NombreGenerico { get; set; } = string.Empty;

        [Required]
        [MaxLength(80)]
        [Column("categoria_terapeutica")]
        public string CategoriaTerapeutica { get; set; } = string.Empty;

        [Required]
        [MaxLength(40)]
        [Column("via_administracion")]
        public string ViaAdministracion { get; set; } = string.Empty;

        [MaxLength(500)]
        [Column("observacion_clinica")]
        public string? ObservacionClinica { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        // Propiedades de navegación (relaciones 1 a muchos)
        public ICollection<Presentacion> Presentaciones { get; set; } = new List<Presentacion>();
        public ICollection<EsquemaDosis> EsquemasDosis { get; set; } = new List<EsquemaDosis>();
        public ICollection<UsuarioFavorito> Favoritos { get; set; } = new List<UsuarioFavorito>();
    }
}