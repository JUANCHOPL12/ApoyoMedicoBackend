using ApoyoClinicoCol.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ApoyoClinicoCol.Api.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor: recibe la configuración de conexión hacia SQL Server desde Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Cada DbSet representa una tabla real en SQL Server
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fuente> Fuentes { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Presentacion> Presentaciones { get; set; }
        public DbSet<EsquemaDosis> EsquemasDosis { get; set; }
        public DbSet<UsuarioFavorito> UsuariosFavoritos { get; set; }

        // OnModelCreating: configura índices únicos, restricciones CHECK y reglas de relación
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Configuración de la tabla Usuarios
            modelBuilder.Entity<Usuario>(entity =>
            {
                // Índices únicos para evitar cuentas duplicadas
                entity.HasIndex(u => u.GoogleSub).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

                // Reglas CHECK de valores permitidos
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Usuarios_Rol", "[rol] IN ('Medico', 'Admin')");
                    t.HasCheckConstraint("CK_Usuarios_EstadoSuscripcion", "[estado_suscripcion] IN ('PruebaGratis', 'Activa', 'Vencida')");
                });
            });

            // 2. Configuración de la tabla Medicamentos
            modelBuilder.Entity<Medicamento>(entity =>
            {
                // Evita registrar dos veces el mismo principio activo
                entity.HasIndex(m => m.NombreGenerico).IsUnique();
            });

            // 3. Configuración de la tabla Esquemas_Dosis (Blindaje Clínico)
            modelBuilder.Entity<EsquemaDosis>(entity =>
            {
                entity.ToTable(t =>
                {
                    // Garantiza coherencia matemática: mínima <= habitual <= máxima
                    t.HasCheckConstraint(
                        "CK_EsquemasDosis_MinMenorHabitual",
                        "[dosis_minima_mg_kg] IS NULL OR [dosis_minima_mg_kg] <= [dosis_habitual_mg_kg]");

                    t.HasCheckConstraint(
                        "CK_EsquemasDosis_HabitualMenorMax",
                        "[dosis_maxima_mg_kg] IS NULL OR [dosis_habitual_mg_kg] <= [dosis_maxima_mg_kg]");

                    // Garantiza valores exactos en tipo de cálculo y estado de validación
                    t.HasCheckConstraint(
                        "CK_EsquemasDosis_TipoCalculo",
                        "[tipo_calculo] IN ('PorDia', 'PorToma')");

                    t.HasCheckConstraint(
                        "CK_EsquemasDosis_Estado",
                        "[estado] IN ('Borrador', 'Validado')");
                });

                // Evita borrado en cascada accidental si se intenta eliminar un medicamento o una fuente
                entity.HasOne(e => e.Medicamento)
                      .WithMany(m => m.EsquemasDosis)
                      .HasForeignKey(e => e.MedicamentoId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Fuente)
                      .WithMany(f => f.EsquemasDosis)
                      .HasForeignKey(e => e.FuenteId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // 4. Configuración de la tabla Usuarios_Favoritos
            modelBuilder.Entity<UsuarioFavorito>(entity =>
            {
                // Índice único compuesto: un médico no puede guardar dos veces el mismo favorito
                entity.HasIndex(uf => new { uf.UsuarioId, uf.MedicamentoId }).IsUnique();
            });
        }
    }
}