using SistemaCarcel.Models;
using Microsoft.EntityFrameworkCore;

namespace SistemaCarcel.Models;

public partial class ProyectoCarcelContext : DbContext
{
    public ProyectoCarcelContext() { }

    public ProyectoCarcelContext(DbContextOptions<ProyectoCarcelContext> options)
        : base(options) { }

    public virtual DbSet<AsignacionPabellonPol> Asignaciones { get; set; }
    public virtual DbSet<Autorizado> Autorizados { get; set; }
    public virtual DbSet<Pabellon> Pabellones { get; set; }
    public virtual DbSet<Personal> Personal { get; set; }
    public virtual DbSet<Recluso> Reclusos { get; set; }
    public virtual DbSet<RegistroVisita> RegistroVisitas { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Visitante> Visitantes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AsignacionPabellonPol>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id_asignacion");

            entity.HasOne(d => d.Personal)
                .WithMany(p => p.Asignaciones)
                .HasForeignKey(d => d.IdPersonal)
                .HasConstraintName("fk_asignacion_personal");

            entity.HasOne(d => d.Pabellon)
                .WithMany()
                .HasForeignKey(d => d.IdPabellon)
                .HasConstraintName("fk_asignacion_pabellon");
        });

        modelBuilder.Entity<Autorizado>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.Visitante)
                .WithOne(v => v.Autorizacion)
                .HasForeignKey<Autorizado>(d => d.IdVisitante)
                .HasConstraintName("fk_auto_visitante");

            entity.HasOne(d => d.Pabellon)
                .WithMany(p => p.Autorizaciones)
                .HasForeignKey(d => d.IdPabellon)
                .HasConstraintName("fk_auto_pabellon");
        });

        modelBuilder.Entity<Pabellon>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombrePb).HasMaxLength(50);
        });

        modelBuilder.Entity<Personal>(entity =>
        {
            entity.HasKey(e => e.IdPersonal);
            entity.Property(e => e.Ci).HasMaxLength(20);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Apellidos).HasMaxLength(100);
            entity.Property(e => e.Sexo).HasMaxLength(10);
            entity.Property(e => e.Cargo).HasMaxLength(50);
        });

        modelBuilder.Entity<Recluso>(entity =>
        {
            entity.HasKey(e => e.IdRecluso);
            entity.Property(e => e.NombreCompleto).HasColumnName("nombre_completo");
            entity.Property(e => e.Ci).HasMaxLength(20).HasColumnName("ci");
            entity.Property(e => e.Edad)
                .HasColumnName("edad");
            entity.Property(e => e.Delito)
                .HasMaxLength(100)
                .HasColumnName("delito");
            entity.Property(e => e.FechaIngreso)
                .HasColumnName("fecha_ingreso");

            entity.HasOne(d => d.Pabellon)
                .WithMany(p => p.Reclusos)
                .HasForeignKey(d => d.IdPabellon)
                .HasConstraintName("fk_pabellon_recluso");
        });

        modelBuilder.Entity<RegistroVisita>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.Visitante)
                .WithMany(v => v.RegistroVisitas)
                .HasForeignKey(d => d.IdVisitante)
                .HasConstraintName("fk_registro_visitante");

            entity.HasOne(d => d.Pabellon)
                .WithMany(p => p.RegistroVisitas)
                .HasForeignKey(d => d.IdPabellon)
                .HasConstraintName("fk_registro_pabellon");

            entity.HasOne(d => d.Personal)
                .WithMany(p => p.RegistroVisitas)
                .HasForeignKey(d => d.IdPersonal)
                .HasConstraintName("fk_registro_personal");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.Rol).HasMaxLength(20);

            entity.HasOne(d => d.Personal)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.IdPersonal)
                .HasConstraintName("fk_user_personal");
        });

        modelBuilder.Entity<Visitante>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombreCompleto).HasMaxLength(100);
            entity.Property(e => e.Ci).HasMaxLength(20);
            entity.Property(e => e.Parentesco).HasMaxLength(50);
            entity.Property(e => e.Imagen).HasMaxLength(200);

            entity.HasOne(d => d.Pabellon)
                .WithMany(p => p.Visitantes)
                .HasForeignKey(d => d.IdPabellon)
                .HasConstraintName("fk_visitante_pabellon");

            entity.HasOne(d => d.Personal)
                .WithMany(p => p.Visitantes)
                .HasForeignKey(d => d.IdPersonal)
                .HasConstraintName("fk_visitante_personal");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}