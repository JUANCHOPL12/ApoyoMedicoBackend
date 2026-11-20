using ApoyoClinicoCol.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ApoyoClinicoCol.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fuente> Fuentes { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Presentacion> Presentaciones { get; set; }
        public DbSet<EsquemaDosis> EsquemasDosis { get; set; }
        public DbSet<UsuarioFavorito> UsuariosFavoritos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Configuración de Usuarios
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(u => u.GoogleSub).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Usuarios_Rol", "[rol] IN ('Medico', 'Admin')");
                    t.HasCheckConstraint("CK_Usuarios_EstadoSuscripcion", "[estado_suscripcion] IN ('PruebaGratis', 'Activa', 'Vencida')");
                });
            });

            // 2. Configuración de Medicamentos
            modelBuilder.Entity<Medicamento>(entity =>
            {
                entity.HasIndex(m => m.NombreGenerico).IsUnique();
            });

            // 3. Configuración de Presentaciones (Evita división por cero y borrado en cascada)
            modelBuilder.Entity<Presentacion>(entity =>
            {
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_Presentaciones_Positivos",
                        "[cantidad_principio_mg] > 0 AND [cantidad_vehiculo] > 0");
                });

                entity.HasOne(p => p.Medicamento)
                      .WithMany(m => m.Presentaciones)
                      .HasForeignKey(p => p.MedicamentoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // 4. Configuración de Esquemas_Dosis (Blindaje Clínico Completo)
            modelBuilder.Entity<EsquemaDosis>(entity =>
            {
                entity.ToTable(t =>
                {
                    // Coherencia matemática: mínima <= habitual <= máxima
                    t.HasCheckConstraint(
                        "CK_EsquemasDosis_MinMenorHabitual",
                        "[dosis_minima_mg_kg] IS NULL OR [dosis_minima_mg_kg] <= [dosis_habitual_mg_kg]");

                    t.HasCheckConstraint(
                        "CK_EsquemasDosis_HabitualMenorMax",
                        "[dosis_maxima_mg_kg] IS NULL OR [dosis_habitual_mg_kg] <= [dosis_maxima_mg_kg]");

                    // Todos los valores de dosis, frecuencias y topes deben ser estrictamente mayores a cero
                    t.HasCheckConstraint(
                        "CK_EsquemasDosis_Positivos",
                        "[dosis_habitual_mg_kg] > 0 AND [frecuencia_horas_defecto] > 0 " +
                        "AND ([dosis_minima_mg_kg] IS NULL OR [dosis_minima_mg_kg] > 0) " +
                        "AND ([dosis_maxima_mg_kg] IS NULL OR [dosis_maxima_mg_kg] > 0) " +
                        "AND ([tope_maximo_dosis_mg] IS NULL OR [tope_maximo_dosis_mg] > 0) " +
                        "AND ([tope_maximo_diario_mg] IS NULL OR [tope_maximo_diario_mg] > 0)");

                    // Coherencia de rangos de edad y peso (mínimo <= máximo y sin valores negativos)
                    t.HasCheckConstraint(
                        "CK_EsquemasDosis_Rangos",
                        "[edad_minima_dias] >= 0 " +
                        "AND ([edad_maxima_dias] IS NULL OR [edad_minima_dias] <= [edad_maxima_dias]) " +
                        "AND ([peso_minimo_kg] IS NULL OR [peso_minimo_kg] > 0) " +
                        "AND ([peso_maximo_kg] IS NULL OR [peso_minimo_kg] IS NULL OR [peso_minimo_kg] <= [peso_maximo_kg])");

                    // Valores permitidos en tipo de cálculo y estado
                    t.HasCheckConstraint(
                        "CK_EsquemasDosis_TipoCalculo",
                        "[tipo_calculo] IN ('PorDia', 'PorToma')");

                    t.HasCheckConstraint(
                        "CK_EsquemasDosis_Estado",
                        "[estado] IN ('Borrador', 'Validado')");

                    // Candado legal: si está en estado 'Validado', exige fecha de revisión y usuario validador
                    t.HasCheckConstraint(
                        "CK_EsquemasDosis_ValidadoCompleto",
                        "[estado] <> 'Validado' OR ([fecha_revision] IS NOT NULL AND [validado_por] IS NOT NULL)");
                });

                // Relaciones protegidas contra borrado en cascada
                entity.HasOne(e => e.Medicamento)
                      .WithMany(m => m.EsquemasDosis)
                      .HasForeignKey(e => e.MedicamentoId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Fuente)
                      .WithMany(f => f.EsquemasDosis)
                      .HasForeignKey(e => e.FuenteId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ValidadoPor)
                      .WithMany()
                      .HasForeignKey(e => e.ValidadoPorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // 5. Configuración de Usuarios_Favoritos
            modelBuilder.Entity<UsuarioFavorito>(entity =>
            {
                entity.HasIndex(uf => new { uf.UsuarioId, uf.MedicamentoId }).IsUnique();
            });
        }
    }
}