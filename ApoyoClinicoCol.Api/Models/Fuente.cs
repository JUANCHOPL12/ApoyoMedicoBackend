using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApoyoClinicoCol.Api.Models
{
    [Table("Fuentes")]
    public class Fuente
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [Column("entidad")]
        public string Entidad { get; set; } = string.Empty;

        [Column("anio")]
        public int Anio { get; set; }

        [MaxLength(300)]
        [Column("enlace")]
        public string? Enlace { get; set; }

        // Propiedad de navegación: una fuente puede respaldar muchos esquemas de dosis
        public ICollection<EsquemaDosis> EsquemasDosis { get; set; } = new List<EsquemaDosis>();
    }
}