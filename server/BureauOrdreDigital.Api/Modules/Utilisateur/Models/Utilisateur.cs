namespace BureauOrdreDigital.Api.Modules.Utilisateur.Models;

public class Utilisateur
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Nom { get; set; } = string.Empty;

    public string Prenom { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Telephone { get; set; }

    public string MotDePasseHash { get; set; } = string.Empty;

    public bool EmailVerifie { get; set; } = false;

    public string? TokenVerification { get; set; }

    public string? TokenResetPassword { get; set; }

    public DateTime? TokenResetExpiration { get; set; }

    public DateTime DateCreation { get; set; } = DateTime.UtcNow;


    // ==========================================
    // OTP - Vérification de connexion par email
    // ==========================================

    /// <summary>
    /// Hash du code OTP envoyé par email.
    /// Le code OTP n'est jamais stocké en clair.
    /// </summary>
    public string? LoginOtpHash { get; set; }

    /// <summary>
    /// Date d'expiration du code OTP.
    /// </summary>
    public DateTime? LoginOtpExpiration { get; set; }

    /// <summary>
    /// Nombre de tentatives OTP échouées.
    /// </summary>
    public int LoginOtpFailedAttempts { get; set; } = 0;


    // ==========================================
    // Challenge temporaire de connexion
    // ==========================================

    /// <summary>
    /// Identifiant temporaire permettant de terminer
    /// l'authentification après validation du mot de passe.
    /// </summary>
    public string? LoginChallenge { get; set; }

    /// <summary>
    /// Date d'expiration du challenge temporaire.
    /// </summary>
    public DateTime? LoginChallengeExpiration { get; set; }
}