using System;
using System.Collections.Generic;
using Gimnasio.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Gimnasio.Infrastructure.Data;

public partial class GimnasioContext : DbContext
{
    public GimnasioContext()
    {
    }

    public GimnasioContext(DbContextOptions<GimnasioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asistencium> Asistencia { get; set; }

    public virtual DbSet<Clase> Clases { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<Instructore> Instructores { get; set; }

    public virtual DbSet<Membresia> Membresias { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioMembresia> UsuarioMembresias { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;database=DbGimnasio;uid=root;pwd=12345678;port=3306", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.3.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Asistencium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.HorarioId, "HorarioId");

            entity.HasIndex(e => e.UsuarioId, "UsuarioId");

            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'Presente'")
                .HasColumnType("enum('Presente','Ausente')");

            entity.HasOne(d => d.Horario).WithMany(p => p.Asistencia)
                .HasForeignKey(d => d.HorarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("asistencia_ibfk_2");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Asistencia)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("asistencia_ibfk_1");
        });

        modelBuilder.Entity<Clase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.InstructorId, "InstructorId");

            entity.Property(e => e.Descripcion).HasMaxLength(30);
            entity.Property(e => e.DuracionMinutos).HasDefaultValueSql("'60'");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)");
            entity.Property(e => e.Nivel)
                .HasDefaultValueSql("'Principiante'")
                .HasColumnType("enum('Principiante','Intermedio','Avanzado')");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Clases)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("clases_ibfk_1");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.ClaseId, "ClaseId");

            entity.Property(e => e.DiaSemana).HasColumnType("enum('Lunes','Martes','Miércoles','Jueves','Viernes')");
            entity.Property(e => e.HoraFin).HasColumnType("time");
            entity.Property(e => e.HoraInicio).HasColumnType("time");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)");
            entity.Property(e => e.Sala).HasMaxLength(10);

            entity.HasOne(d => d.Clase).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.ClaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("horarios_ibfk_1");
        });

        modelBuilder.Entity<Instructore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Especialidad).HasMaxLength(100);
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Membresia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.ClasesIncluidas).HasDefaultValueSql("'0'");
            entity.Property(e => e.Descripcion).HasMaxLength(20);
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)");
            entity.Property(e => e.Precio).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.IsActive).HasColumnType("bit(1)");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<UsuarioMembresia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Usuario_Membresias");

            entity.HasIndex(e => e.MembresiaId, "MembresiaId");

            entity.HasIndex(e => e.UsuarioId, "UsuarioId");

            entity.Property(e => e.ClasesRestantes).HasDefaultValueSql("'0'");
            entity.Property(e => e.Estado).HasColumnType("enum('Activa','Expirada','Cancelada')");
            entity.Property(e => e.MetodoPago).HasColumnType("enum('Efectivo','Tarjeta','QR')");
            entity.Property(e => e.PrecioPagado).HasPrecision(10, 2);

            entity.HasOne(d => d.Membresia).WithMany(p => p.UsuarioMembresia)
                .HasForeignKey(d => d.MembresiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_membresias_ibfk_2");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioMembresia)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_membresias_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


