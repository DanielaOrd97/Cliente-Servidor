using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ITESRCLibrosAPI.Models.Entities;

public partial class ItesrcneLibrosContext : DbContext
{
    public ItesrcneLibrosContext()
    {
    }

    public ItesrcneLibrosContext(DbContextOptions<ItesrcneLibrosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Libros> Libros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=204.93.216.11;database=itesrcne_libros;user=itesrcne_libuser;password=8Ex298hJU3Ts", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.29-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Libros>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("libros");

            entity.HasIndex(e => e.Id, "libros_Id_IDX");

            entity.Property(e => e.Autor).HasMaxLength(100);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Portada).HasMaxLength(100);
            entity.Property(e => e.Titulo).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
