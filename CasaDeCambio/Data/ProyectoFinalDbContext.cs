using System;
using System.Collections.Generic;
using CasaDeCambio.Models;
using Microsoft.EntityFrameworkCore;

namespace CasaDeCambio.Data;

public partial class ProyectoFinalDbContext : DbContext
{
    public ProyectoFinalDbContext(DbContextOptions<ProyectoFinalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CambiosDivisa> CambiosDivisas { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CambiosDivisa>(entity =>
        {
            entity.Property(e => e.FechaHoraCambio).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.CambiosDivisas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CambiosDivisas_Personas");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
