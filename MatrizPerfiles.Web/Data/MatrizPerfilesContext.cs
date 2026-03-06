using MatrizPerfiles.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace MatrizPerfiles.Web.Data
{
    public class MatrizPerfilesContext : DbContext
    {
        public MatrizPerfilesContext(DbContextOptions<MatrizPerfilesContext> options) : base(options)
        {
        }

        public DbSet<Sistema> Sistemas => Set<Sistema>();
        public DbSet<Puesto> Puestos => Set<Puesto>();
        public DbSet<PbiMatrizPerfil> PbiMatrizPerfiles => Set<PbiMatrizPerfil>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Sistema Configuration
            modelBuilder.Entity<Sistema>(entity =>
            {
                entity.ToTable("Sistemas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            });

            // Puesto Configuration
            modelBuilder.Entity<Puesto>(entity =>
            {
                entity.ToTable("Puestos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            });

            // PbiMatrizPerfil Configuration
            modelBuilder.Entity<PbiMatrizPerfil>(entity =>
            {
                entity.ToTable("PbiMatrizPerfiles");
                
                // Primary Key (Identity)
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn(1, 1);

                // Code constraints
                entity.Property(e => e.Codigo).HasMaxLength(100);

                // Foreign Keys
                entity.HasOne(d => d.Sistema)
                    .WithMany()
                    .HasForeignKey(d => d.SistemaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Puesto)
                    .WithMany()
                    .HasForeignKey(d => d.PuestoId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Required base fields logic
                entity.Property(e => e.Empresa).IsRequired();
                
                // Indices requested
                entity.HasIndex(e => e.Codigo).HasDatabaseName("IX_PbiMatrizPerfil_Codigo");
                entity.HasIndex(e => e.FuncionRolAsociado).HasDatabaseName("IX_PbiMatrizPerfil_FuncionRolAsociado");
                entity.HasIndex(e => e.SistemaId).HasDatabaseName("IX_PbiMatrizPerfil_SistemaId");
                entity.HasIndex(e => e.Vistas).HasDatabaseName("IX_PbiMatrizPerfil_Vistas");
                
                // Read-only UTC Date logic
                entity.Property(e => e.FechaCreacion)
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Data Seeding
            modelBuilder.Entity<Sistema>().HasData(
                new Sistema { Id = 1, Nombre = "Sistema Core" },
                new Sistema { Id = 2, Nombre = "CRM" },
                new Sistema { Id = 3, Nombre = "ERP Financiero" },
                new Sistema { Id = 4, Nombre = "Banca Móvil" }
            );

            modelBuilder.Entity<Puesto>().HasData(
                new Puesto { Id = 1, Nombre = "Analista de Crédito" },
                new Puesto { Id = 2, Nombre = "Gerente de Sucursal" },
                new Puesto { Id = 3, Nombre = "Cajero" },
                new Puesto { Id = 4, Nombre = "Auditor Interno" }
            );
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<PbiMatrizPerfil>()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                entry.Entity.FechaCreacion = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
