using BureauOrdreDigital.Api.Modules.Utilisateur.Models;
using Microsoft.EntityFrameworkCore;

namespace BureauOrdreDigital.Api.Shared.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Utilisateur> Utilisateurs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.ToTable("Utilisateurs");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Nom)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Prenom)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(u => u.Telephone)
                .HasMaxLength(20);

            entity.Property(u => u.MotDePasseHash)
                .IsRequired();

            entity.Property(u => u.EmailVerifie)
                .IsRequired();

            entity.Property(u => u.TokenVerification)
                .HasMaxLength(255);

            entity.Property(u => u.TokenResetPassword)
                .HasMaxLength(255);

            entity.Property(u => u.TokenResetExpiration);

            entity.Property(u => u.DateCreation)
                .IsRequired();

            entity.HasIndex(u => u.Email)
                .IsUnique();
        });
    }
}