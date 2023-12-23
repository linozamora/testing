using Ficohsa.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Ficohsa.DAO
{
    public partial class FicohsaContext : DbContext
    {
        public FicohsaContext()
        {
        }

        public FicohsaContext(DbContextOptions<FicohsaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Banco> Banco { get; set; } = null!;
        public virtual DbSet<Canal> Canal { get; set; } = null!;
        public virtual DbSet<Catalogo> Catalogo { get; set; } = null!;
        public virtual DbSet<Moneda> Moneda { get; set; } = null!;
        public virtual DbSet<Orquestacion> Orquestacion { get; set; } = null!;
        public virtual DbSet<Pais> Pais { get; set; } = null!;
        public virtual DbSet<Parametro> Parametro { get; set; } = null!;
        public virtual DbSet<ParticipanteIndirecto> ParticipanteIndirecto { get; set; } = null!;
        public virtual DbSet<Tabla> Tabla { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("Main");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Banco>(entity =>
            {
                entity.HasKey(e => e.IdBanco);

                entity.HasIndex(e => new { e.IdPais, e.CodigoACHMonedaExtranjera }, "UK_Banco_IdPais_CodigoACHMonedaExtranjera")
                    .IsUnique();

                entity.HasIndex(e => new { e.IdPais, e.CodigoACHMonedaLocal }, "UK_Banco_IdPais_CodigoACHMonedaLocal")
                    .IsUnique();

                entity.HasIndex(e => new { e.IdPais, e.NombreBanco }, "UK_Banco_IdPais_NombreBanco")
                    .IsUnique();

                entity.Property(e => e.AHMensajeError)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.CCMensajeError)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoACHMonedaExtranjera)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoACHMonedaLocal)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoCoreBancario)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.NombreBanco)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.PTOMensajeError)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.TCMensajeError)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Canal>(entity =>
            {
                entity.HasKey(e => e.IdCanal);

                entity.HasIndex(e => new { e.IdPais, e.Nombre }, "UK_Canal_IdPais_Nombre")
                    .IsUnique();

                entity.HasIndex(e => e.Usuario, "UK_Canal_Usuario")
                    .IsUnique();

                entity.Property(e => e.Estado)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.TipoCanal)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Canal)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Canal_Pais");
            });

            modelBuilder.Entity<Catalogo>(entity =>
            {
                entity.HasKey(e => e.IdCatalogo);

                entity.HasIndex(e => new { e.IdTabla, e.IdPais, e.Codigo }, "UK_Catalogo_IdTabla_IdPais_Codigo")
                    .IsUnique();

                entity.Property(e => e.Codigo)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Valor)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Catalogo)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Catalogo_Pais");

                entity.HasOne(d => d.IdTablaNavigation)
                    .WithMany(p => p.Catalogo)
                    .HasForeignKey(d => d.IdTabla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Catalogo_Tabla");
            });

            modelBuilder.Entity<Moneda>(entity =>
            {
                entity.HasKey(e => e.IdMoneda);

                entity.HasIndex(e => new { e.IdPais, e.CodigoISO }, "UK_Moneda_IdPais_CodigoISO")
                    .IsUnique();

                entity.HasIndex(e => new { e.IdPais, e.Nombre }, "UK_Moneda_IdPais_Nombre")
                    .IsUnique();

                entity.Property(e => e.CodigoISO)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Simbolo)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.TipoCambio).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Moneda)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Moneda_Pais");
            });

            modelBuilder.Entity<Orquestacion>(entity =>
            {
                entity.HasKey(e => e.IdOrquestacion);

                entity.Property(e => e.BancoDestino)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Canal)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoActual)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoFallido)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoRecuperacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.MessageType)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Monto)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.PILocal)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Producto)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.SiguientePaso)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Topico)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Orquestacion)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orquestacion_Pais");
            });

            modelBuilder.Entity<Pais>(entity =>
            {
                entity.HasKey(e => e.IdPais);

                entity.HasIndex(e => e.CodigoISO, "UK_Pais_CodigoISO")
                    .IsUnique();

                entity.HasIndex(e => e.Nombre, "UK_Pais_Nombre")
                    .IsUnique();

                entity.Property(e => e.CodigoISO)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Parametro>(entity =>
            {
                entity.HasKey(e => e.IdParametro);

                entity.HasIndex(e => new { e.IdPais, e.Nombre }, "UK_Parametro_IdPais_Nombre")
                    .IsUnique();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Valor)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Parametro)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Parametro_Pais");
            });

            modelBuilder.Entity<ParticipanteIndirecto>(entity =>
            {
                entity.HasKey(e => e.IdParticipanteIndirecto);

                entity.HasIndex(e => new { e.IdPais, e.Nombre }, "UK_ParticipanteIndirecto_IdPais_Nombre")
                    .IsUnique();

                entity.Property(e => e.AHMensajeError)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.APIPassword)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.APIUser)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.CCMensajeError)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoACHMonedaExtranjera)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoACHMonedaLocal)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoCoreBancario)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.CuentaCoreBancarioMonedaExtranjera)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.CuentaCoreBancarioMonedaLocal)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.CuentaInternaCoreBancarioMonedaExtranjera)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.CuentaInternaCoreBancarioMonedaLocal)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.PTOMensajeError)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.TCMensajeError)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.URL)
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tabla>(entity =>
            {
                entity.HasKey(e => e.IdTabla);

                entity.HasIndex(e => e.Nombre, "UK_Tabla_Nombre")
                    .IsUnique();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioModificacion)
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
