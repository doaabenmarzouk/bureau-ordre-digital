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

    // ================================
    // DbSets
    // ================================

    public DbSet<Utilisateur> Utilisateurs { get; set; }

    public DbSet<Courrier> Courriers { get; set; }

    public DbSet<TypeCourrier> TypesCourrier { get; set; }

    public DbSet<Organisme> Organismes { get; set; }

    public DbSet<StatutCourrier> StatutsCourrier { get; set; }

    public DbSet<PieceJointe> PiecesJointes { get; set; }

    public DbSet<HistoriqueAction> HistoriqueActions { get; set; }

    public DbSet<RevokedToken> RevokedTokens { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // =========================================================
        // UTILISATEUR
        // =========================================================

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


        // =========================================================
        // ORGANISME
        // =========================================================
          // =========================================================
// ORGANISME
// =========================================================

modelBuilder.Entity<Organisme>(entity =>
{
    entity.ToTable("Organismes");

    entity.HasKey(o => o.Id);

    entity.Property(o => o.Nom)
        .IsRequired()
        .HasMaxLength(200);

    entity.Property(o => o.NomAbrege)
        .HasMaxLength(50);

    entity.Property(o => o.Type)
        .HasMaxLength(100);

    entity.Property(o => o.Pays)
        .HasMaxLength(100);

    entity.Property(o => o.Ville)
        .HasMaxLength(100);

    entity.Property(o => o.Scope)
        .HasMaxLength(100);

    entity.Property(o => o.Mode)
        .HasMaxLength(100);

    entity.Property(o => o.Marche)
        .HasMaxLength(100);

    entity.Property(o => o.Partenaire)
        .HasMaxLength(150);

    entity.Property(o => o.Contrat)
        .HasMaxLength(100);

    entity.Property(o => o.DateAnniversaire);

    entity.Property(o => o.VersionElise)
        .HasMaxLength(100);

    entity.Property(o => o.NombreUtilisateurs);

    entity.Property(o => o.MontantContrat)
        .HasPrecision(18, 2);

    entity.Property(o => o.ChefProjetCsm)
        .HasMaxLength(200);

    entity.Property(o => o.CodeDivalto)
        .HasMaxLength(100);

    entity.Property(o => o.NomContact)
        .HasMaxLength(200);

    entity.Property(o => o.MailContact)
        .HasMaxLength(255);

    entity.Property(o => o.TelContact)
        .HasMaxLength(50);

    entity.Property(o => o.FonctionContact)
        .HasMaxLength(150);

    entity.Property(o => o.Actif)
        .IsRequired();
});
             

        // =========================================================
        // COURRIER
        // =========================================================

        modelBuilder.Entity<Courrier>(entity =>
        {
            entity.ToTable("Courriers");

            entity.HasKey(c => c.Id);


            // Numéro de référence
            entity.Property(c => c.NumReference)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasIndex(c => c.NumReference)
                .IsUnique();


            // Utilisateur
            entity.Property(c => c.UtilisateurId)
                .IsRequired();


            // Type courrier
            entity.Property(c => c.TypeCourrierId)
                .IsRequired();


            // Organisme destinataire
            entity.Property(c => c.OrganismeId)
                .IsRequired();


            // Statut
            entity.Property(c => c.StatutId)
                .IsRequired();


            // Objet
            entity.Property(c => c.Objet)
                .IsRequired()
                .HasMaxLength(200);


            // Description
            entity.Property(c => c.Description)
                .IsRequired();


            // Dates
            entity.Property(c => c.DateSoumission)
                .IsRequired();

            entity.Property(c => c.DateDerniereMaj)
                .IsRequired();


            // Relation avec Utilisateur
            entity.HasOne(c => c.Utilisateur)
                .WithMany()
                .HasForeignKey(c => c.UtilisateurId)
                .OnDelete(DeleteBehavior.Restrict);


            // Relation avec TypeCourrier
            entity.HasOne(c => c.TypeCourrier)
                .WithMany()
                .HasForeignKey(c => c.TypeCourrierId)
                .OnDelete(DeleteBehavior.Restrict);


            // Relation avec Organisme
            entity.HasOne(c => c.Organisme)
                .WithMany()
                .HasForeignKey(c => c.OrganismeId)
                .OnDelete(DeleteBehavior.Restrict);


            // Relation avec Statut
            entity.HasOne(c => c.StatutCourrier)
                .WithMany()
                .HasForeignKey(c => c.StatutId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        // =========================================================
        // TYPE COURRIER
        // =========================================================

        modelBuilder.Entity<TypeCourrier>(entity =>
        {
            entity.ToTable("TypeCourrier");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Libelle)
                .IsRequired()
                .HasMaxLength(100);
        });


        // =========================================================
        // STATUT COURRIER
        // =========================================================

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


        // =========================================================
        // PIECES JOINTES
        // =========================================================

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


        // =========================================================
        // HISTORIQUE ACTIONS
        // =========================================================

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