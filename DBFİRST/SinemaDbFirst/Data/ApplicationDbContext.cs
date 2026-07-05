using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace dbfirstProjem.Models;

public partial class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Biletler> Biletlers { get; set; }

    public virtual DbSet<Filmler> Filmlers { get; set; }

    public virtual DbSet<Koltuklar> Koltuklars { get; set; }

    public virtual DbSet<Salonlar> Salonlars { get; set; }

    public virtual DbSet<Seanslar> Seanslars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=sinema;User Id=sa;Password=GucluSifre123!;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Biletler>(entity =>
        {
            entity.HasKey(e => e.BiletId).HasName("PK__Biletler__9518DF896CA75B02");

            entity.ToTable("Biletler");

            entity.HasIndex(e => new { e.SeansId, e.KoltukId }, "UQ_Seans_Koltuk").IsUnique();

            entity.Property(e => e.Fiyat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SatisTarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Koltuk).WithMany(p => p.Biletlers)
                .HasForeignKey(d => d.KoltukId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Biletler_Koltuklar");

            entity.HasOne(d => d.Seans).WithMany(p => p.Biletlers)
                .HasForeignKey(d => d.SeansId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Biletler_Seanslar");
        });

        modelBuilder.Entity<Filmler>(entity =>
        {
            entity.HasKey(e => e.FilmId).HasName("PK__Filmler__6D1D217C4CA355AA");

            entity.ToTable("Filmler");

            entity.Property(e => e.Ad).HasMaxLength(100);
            entity.Property(e => e.Tur).HasMaxLength(50);
            entity.Property(e => e.Fiyat).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Koltuklar>(entity =>
        {
            entity.HasKey(e => e.KoltukId).HasName("PK__Koltukla__77FF0E77382EAB74");

            entity.ToTable("Koltuklar");

            entity.Property(e => e.Sira).HasMaxLength(5);

            entity.HasOne(d => d.Salon).WithMany(p => p.Koltuklars)
                .HasForeignKey(d => d.SalonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Koltuklar_Salonlar");
        });

        modelBuilder.Entity<Salonlar>(entity =>
        {
            entity.HasKey(e => e.SalonId).HasName("PK__Salonlar__5E25586150AC260A");

            entity.ToTable("Salonlar");

            entity.Property(e => e.Ad)
                  .HasMaxLength(50)
                  .HasColumnName("SalonAd");
        });

        modelBuilder.Entity<Seanslar>(entity =>
        {
            entity.HasKey(e => e.SeansId).HasName("PK__Seanslar__0359BD004DB462D0");

            entity.ToTable("Seanslar");

            entity.Property(e => e.BaslamaZamani).HasColumnType("datetime");

            entity.HasOne(d => d.Film).WithMany(p => p.Seanslars)
                .HasForeignKey(d => d.FilmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seanslar_Filmler");

            entity.HasOne(d => d.Salon).WithMany(p => p.Seanslars)
                .HasForeignKey(d => d.SalonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seanslar_Salonlar");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
