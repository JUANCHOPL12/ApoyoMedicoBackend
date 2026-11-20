using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApoyoClinicoCol.Api.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("google_sub")]
        public string GoogleSub { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [Column("nombre_completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("rol")]
        public string Rol { get; set; } = "Medico";

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; }

        [Column("fin_prueba_gratis")]
        public DateTime FinPruebaGratis { get; set; }

        [Required]
        [MaxLength(30)]
        [Column("estado_suscripcion")]
        public string EstadoSuscripcion { get; set; } = "PruebaGratis";

        [Column("fin_suscripcion")]
        public DateTime? FinSuscripcion { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        // Propiedad de navegación: un usuario puede tener muchos medicamentos favoritos
        public ICollection<UsuarioFavorito> Favoritos { get; set; } = new List<UsuarioFavorito>();
    }
}