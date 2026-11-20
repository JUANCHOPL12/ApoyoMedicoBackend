using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApoyoClinicoCol.Api.Models
{
    [Table("Usuarios_Favoritos")]
    public class UsuarioFavorito
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("medicamento_id")]
        public int MedicamentoId { get; set; }

        [Column("fecha_agregado")]
        public DateTime FechaAgregado { get; set; }

        // Relaciones hacia Usuarios y Medicamentos
        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }

        [ForeignKey(nameof(MedicamentoId))]
        public Medicamento? Medicamento { get; set; }
    }
}