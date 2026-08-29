using BureauOrdreDigital.Api.Modules.Courrier.Models;
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
    public DbSet<Courrier> Courriers { get; set; }
    public DbSet<TypeCourrier> TypesCourrier { get; set; }
    public DbSet<ServiceDestinataire> ServicesDestinataires { get; set; }
    public DbSet<StatutCourrier> StatutsCourrier { get; set; }
    public DbSet<PieceJointe> PiecesJointes { get; set; }
    public DbSet<HistoriqueAction> HistoriqueActions { get; set; }
    public DbSet<RevokedToken> RevokedTokens { get; set; }

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

        modelBuilder.Entity<Courrier>(entity =>
        {
            entity.ToTable("Courriers");
            entity.HasKey(c => c.Id);

            entity.Property(c => c.NumReference)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasIndex(c => c.NumReference)
                .IsUnique();

            entity.Property(c => c.UtilisateurId)
                .IsRequired();

            entity.Property(c => c.TypeCourrierId)
                .IsRequired();

            entity.Property(c => c.ServiceDestinataireId)
                .IsRequired();

            entity.Property(c => c.StatutId)
                .IsRequired();

            entity.Property(c => c.Objet)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(c => c.Description)
                .IsRequired();

            entity.Property(c => c.DateSoumission)
                .IsRequired();

            entity.Property(c => c.DateDerniereMaj)
                .IsRequired();

            entity.HasOne(c => c.Utilisateur)
                .WithMany()
                .HasForeignKey(c => c.UtilisateurId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.TypeCourrier)
                .WithMany()
                .HasForeignKey(c => c.TypeCourrierId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.ServiceDestinataire)
                .WithMany()
                .HasForeignKey(c => c.ServiceDestinataireId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.StatutCourrier)
                .WithMany()
                .HasForeignKey(c => c.StatutId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TypeCourrier>(entity =>
        {
            entity.ToTable("TypeCourrier");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Libelle)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<StatutCourrier>(entity =>
        {
            entity.ToTable("StatutCourrier");
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Code)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasIndex(s => s.Code)
                .IsUnique();

            entity.Property(s => s.Libelle)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(s => s.Ordre)
                .IsRequired();
        });

       modelBuilder.Entity<ServiceDestinataire>(entity =>
{
    entity.ToTable("ServiceDestinataire");
    entity.HasKey(s => s.Id);

    entity.Property(s => s.NomService)
        .IsRequired()
        .HasMaxLength(150);

    entity.Property(s => s.NomOrganisme)
        .IsRequired()
        .HasMaxLength(150);

    entity.Property(s => s.Ville)
        .IsRequired()
        .HasMaxLength(100);

    entity.Property(s => s.TypeOrganisme)
        .IsRequired()
        .HasMaxLength(100);

    entity.Property(s => s.Actif)
        .IsRequired();
});

        modelBuilder.Entity<PieceJointe>(entity =>
        {
            entity.ToTable("PiecesJointes");
            entity.HasKey(p => p.Id);

            entity.Property(p => p.NomFichier)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(p => p.UrlStockage)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(p => p.TailleOctets)
                .IsRequired();

            entity.Property(p => p.Format)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(p => p.DateUpload)
                .IsRequired();

            entity.HasOne(p => p.Courrier)
                .WithMany()
                .HasForeignKey(p => p.CourrierId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HistoriqueAction>(entity =>
        {
            entity.ToTable("HistoriqueActions");
            entity.HasKey(h => h.Id);

            entity.Property(h => h.DateAction)
                .IsRequired();

            entity.Property(h => h.Description)
                .HasMaxLength(500);

            entity.HasOne(h => h.Courrier)
                .WithMany()
                .HasForeignKey(h => h.CourrierId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(h => h.StatutCourrier)
                .WithMany()
                .HasForeignKey(h => h.StatutId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}