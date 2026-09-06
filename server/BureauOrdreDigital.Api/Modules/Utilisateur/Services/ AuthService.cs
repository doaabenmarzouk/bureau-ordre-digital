using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;
using BureauOrdreDigital.Api.Modules.Utilisateur.Models;
using BureauOrdreDigital.Api.Shared.Data;
using BureauOrdreDigital.Api.Shared.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BureauOrdreDigital.Api.Modules.Utilisateur.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public AuthService(
        AppDbContext context,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _context = context;
        _emailService = emailService;
        _configuration = configuration;
    }


    // =========================================================
    // REGISTER
    // =========================================================

    public async Task<Models.Utilisateur> Register(RegisterDto dto)
    {
        bool emailExiste = await _context.Utilisateurs
            .AnyAsync(u => u.Email == dto.Email);

        if (emailExiste)
        {
            throw new InvalidOperationException(
                "Cet email est déjà utilisé."
            );
        }

        var utilisateur = new Models.Utilisateur
        {
            Nom = dto.Nom,
            Prenom = dto.Prenom,
            Email = dto.Email,
            Telephone = dto.Telephone,

            MotDePasseHash =
                BCrypt.Net.BCrypt.HashPassword(dto.MotDePasse),

            TokenVerification = Guid.NewGuid().ToString(),

            DateCreation = DateTime.UtcNow,

            LoginOtpHash = null,
            LoginOtpExpiration = null,
            LoginOtpFailedAttempts = 0,

            LoginChallenge = null,
            LoginChallengeExpiration = null
        };

        _context.Utilisateurs.Add(utilisateur);

        await _context.SaveChangesAsync();

        var lien =
            $"http://localhost:5029/api/auth/verify-email?token={utilisateur.TokenVerification}";

        var corps = ConstruireTemplateVerification(lien);

        await _emailService.SendEmailAsync(
            utilisateur.Email,
            "Vérifiez votre adresse email",
            corps
        );

        return utilisateur;
    }


    // =========================================================
    // LOGIN
    // =========================================================

    public async Task<LoginResponseDto> Login(LoginDto dto)
    {
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (utilisateur is null)
        {
            throw new InvalidOperationException(
                "Email ou mot de passe incorrect."
            );
        }

        bool motDePasseValide =
            BCrypt.Net.BCrypt.Verify(
                dto.MotDePasse,
                utilisateur.MotDePasseHash
            );

        if (!motDePasseValide)
        {
            throw new InvalidOperationException(
                "Email ou mot de passe incorrect."
            );
        }

        if (!utilisateur.EmailVerifie)
        {
            throw new InvalidOperationException(
                "Veuillez vérifier votre adresse email avant de vous connecter."
            );
        }


        // =====================================================
        // GÉNÉRATION DU CHALLENGE
        // =====================================================

        var challengeBytes =
            RandomNumberGenerator.GetBytes(32);

        var challenge =
            Convert.ToBase64String(challengeBytes);


        // =====================================================
        // GÉNÉRATION DU CODE OTP
        // =====================================================

        int codeOtp =
            RandomNumberGenerator.GetInt32(
                100000,
                1000000
            );

        string codeOtpString =
            codeOtp.ToString();


        // =====================================================
        // STOCKAGE SÉCURISÉ
        // =====================================================

        utilisateur.LoginChallenge = challenge;

        utilisateur.LoginChallengeExpiration =
            DateTime.UtcNow.AddMinutes(5);

        // On ne stocke jamais le OTP en clair
        utilisateur.LoginOtpHash =
            BCrypt.Net.BCrypt.HashPassword(
                codeOtpString
            );

        utilisateur.LoginOtpExpiration =
            DateTime.UtcNow.AddMinutes(5);

        utilisateur.LoginOtpFailedAttempts = 0;

        await _context.SaveChangesAsync();


        // =====================================================
        // ENVOI DU OTP PAR EMAIL
        // =====================================================

        var corps =
            ConstruireTemplateOtp(
                utilisateur.Prenom,
                codeOtpString
            );

        await _emailService.SendEmailAsync(
            utilisateur.Email,
            "Votre code de connexion",
            corps
        );


        // =====================================================
        // PAS DE JWT À CE STADE
        // =====================================================

        return new LoginResponseDto
        {
            Id = utilisateur.Id,
            Nom = utilisateur.Nom,
            Prenom = utilisateur.Prenom,
            Email = utilisateur.Email,
            Telephone = utilisateur.Telephone,

            Token = null,

            RequiresOtp = true,

            Challenge = challenge
        };
    }


    // =========================================================
    // VERIFY OTP
    // =========================================================

    public async Task<LoginResponseDto> VerifyOtp(
        VerifyOtpDto dto)
    {
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(
                u => u.LoginChallenge == dto.Challenge
            );

        if (utilisateur is null)
        {
            throw new InvalidOperationException(
                "Session de connexion invalide."
            );
        }


        // =====================================================
        // VÉRIFIER L'EXPIRATION DU CHALLENGE
        // =====================================================

        if (!utilisateur.LoginChallengeExpiration.HasValue ||
            utilisateur.LoginChallengeExpiration.Value <= DateTime.UtcNow)
        {
            utilisateur.LoginChallenge = null;
            utilisateur.LoginChallengeExpiration = null;
            utilisateur.LoginOtpHash = null;
            utilisateur.LoginOtpExpiration = null;
            utilisateur.LoginOtpFailedAttempts = 0;

            await _context.SaveChangesAsync();

            throw new InvalidOperationException(
                "Votre session de connexion a expiré. Veuillez vous reconnecter."
            );
        }


        // =====================================================
        // VÉRIFIER L'EXPIRATION DU OTP
        // =====================================================

        if (!utilisateur.LoginOtpExpiration.HasValue ||
            utilisateur.LoginOtpExpiration.Value <= DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "Le code OTP a expiré. Veuillez vous reconnecter."
            );
        }


        // =====================================================
        // LIMITE DE TENTATIVES
        // =====================================================

        if (utilisateur.LoginOtpFailedAttempts >= 5)
        {
            throw new InvalidOperationException(
                "Trop de tentatives incorrectes. Veuillez vous reconnecter."
            );
        }


        // =====================================================
        // VÉRIFICATION DU CODE
        // =====================================================

        bool codeValide =
            !string.IsNullOrWhiteSpace(
                utilisateur.LoginOtpHash
            )
            &&
            BCrypt.Net.BCrypt.Verify(
                dto.Code,
                utilisateur.LoginOtpHash
            );


        if (!codeValide)
        {
            utilisateur.LoginOtpFailedAttempts++;

            await _context.SaveChangesAsync();

            if (utilisateur.LoginOtpFailedAttempts >= 5)
            {
                throw new InvalidOperationException(
                    "Trop de tentatives incorrectes. Veuillez vous reconnecter."
                );
            }

            throw new InvalidOperationException(
                "Code OTP incorrect."
            );
        }


        // =====================================================
        // OTP CORRECT
        // =====================================================

        utilisateur.LoginOtpHash = null;
        utilisateur.LoginOtpExpiration = null;
        utilisateur.LoginOtpFailedAttempts = 0;

        utilisateur.LoginChallenge = null;
        utilisateur.LoginChallengeExpiration = null;

        await _context.SaveChangesAsync();


        // =====================================================
        // GÉNÉRATION DU JWT
        // =====================================================

        var token =
            GenererToken(utilisateur);


        return new LoginResponseDto
        {
            Id = utilisateur.Id,
            Nom = utilisateur.Nom,
            Prenom = utilisateur.Prenom,
            Email = utilisateur.Email,
            Telephone = utilisateur.Telephone,

            Token = token,

            RequiresOtp = false,

            Challenge = null
        };
    }


    // =========================================================
    // VERIFY EMAIL
    // =========================================================

    public async Task VerifyEmail(string token)
    {
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(
                u => u.TokenVerification == token
            );

        if (utilisateur is null)
        {
            throw new InvalidOperationException(
                "Token invalide."
            );
        }

        utilisateur.EmailVerifie = true;
        utilisateur.TokenVerification = null;

        await _context.SaveChangesAsync();
    }


    // =========================================================
    // FORGOT PASSWORD
    // =========================================================

    public async Task ForgotPassword(ForgotPasswordDto dto)
    {
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(
                u => u.Email == dto.Email
            );

        if (utilisateur is null)
        {
            return;
        }

        var token =
            Guid.NewGuid().ToString("N");

        utilisateur.TokenResetPassword = token;

        utilisateur.TokenResetExpiration =
            DateTime.UtcNow.AddMinutes(30);

        await _context.SaveChangesAsync();

        var lien =
            $"http://localhost:5173/reset-password?token={token}";

        var corps =
            ConstruireTemplateResetPassword(lien);

        await _emailService.SendEmailAsync(
            utilisateur.Email,
            "Réinitialisation de votre mot de passe",
            corps
        );
    }


    // =========================================================
    // RESET PASSWORD
    // =========================================================

    public async Task ResetPassword(
        ResetPasswordDto dto)
    {
        if (dto.NouveauMotDePasse !=
            dto.ConfirmationMotDePasse)
        {
            throw new InvalidOperationException(
                "Les mots de passe ne correspondent pas."
            );
        }

        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(
                u => u.TokenResetPassword == dto.Token
            );

        if (utilisateur is null)
        {
            throw new InvalidOperationException(
                "Lien de réinitialisation invalide."
            );
        }

        if (utilisateur.TokenResetExpiration <
            DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "Le lien de réinitialisation a expiré."
            );
        }

        utilisateur.MotDePasseHash =
            BCrypt.Net.BCrypt.HashPassword(
                dto.NouveauMotDePasse
            );

        utilisateur.TokenResetPassword = null;
        utilisateur.TokenResetExpiration = null;

        await _context.SaveChangesAsync();
    }


    // =========================================================
    // GENERATE JWT
    // =========================================================

    private string GenererToken(
        Models.Utilisateur utilisateur)
    {
        var claims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                utilisateur.Id.ToString()
            ),

            new(
                ClaimTypes.Email,
                utilisateur.Email
            )
        };


        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!
            )
        );


        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );


        var expirationMinutes =
            double.Parse(
                _configuration["Jwt:ExpirationMinutes"]!
            );


        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,

            expires:
                DateTime.UtcNow.AddMinutes(
                    expirationMinutes
                ),

            signingCredentials: credentials
        );


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }


    // =========================================================
    // TEMPLATE OTP
    // =========================================================

    private static string ConstruireTemplateOtp(
        string prenom,
        string code)
    {
        return $$"""
        <div style="
            font-family: Arial, sans-serif;
            background-color: #f4f6f8;
            padding: 40px 20px;
        ">

            <div style="
                max-width: 560px;
                margin: 0 auto;
                background-color: #ffffff;
                border-radius: 12px;
                padding: 40px;
                text-align: center;
                border: 1px solid #e5e7eb;
            ">

                <h2 style="
                    color: #91a91c;
                    margin-bottom: 20px;
                ">
                    Bureau d'Ordre Digital
                </h2>

                <h3 style="
                    color: #111827;
                    font-size: 22px;
                    margin-bottom: 15px;
                ">
                    Code de connexion
                </h3>

                <p style="
                    color: #4b5563;
                    font-size: 15px;
                    line-height: 1.7;
                ">
                    Bonjour {{prenom}},
                </p>

                <p style="
                    color: #4b5563;
                    font-size: 15px;
                    line-height: 1.7;
                ">
                    Voici votre code de vérification pour vous connecter :
                </p>

                <div style="
                    display: inline-block;
                    margin: 25px 0;
                    padding: 16px 30px;
                    background-color: #f3f4f6;
                    border-radius: 8px;
                    font-size: 30px;
                    font-weight: bold;
                    letter-spacing: 8px;
                    color: #1e3a8a;
                ">
                    {{code}}
                </div>

                <p style="
                    color: #6b7280;
                    font-size: 13px;
                    line-height: 1.6;
                ">
                    Ce code est valable pendant 5 minutes.
                </p>

                <p style="
                    color: #9ca3af;
                    font-size: 12px;
                    line-height: 1.6;
                ">
                    Si vous n'êtes pas à l'origine de cette tentative
                    de connexion, vous pouvez ignorer cet email.
                </p>

                <hr style="
                    border: 0;
                    border-top: 1px solid #e5e7eb;
                    margin: 30px 0;
                ">

                <p style="
                    color: #9ca3af;
                    font-size: 11px;
                ">
                    © 2026 Bureau d'Ordre Digital
                </p>

            </div>

        </div>
        """;
    }


    // =========================================================
    // EMAIL VERIFICATION TEMPLATE
    // =========================================================

    private static string ConstruireTemplateVerification(
        string lien)
    {
        return $$"""
        <div style="font-family: Arial, sans-serif; background-color: #f4f6f8; padding: 40px 20px;">
            <div style="max-width: 560px; margin: 0 auto; background-color: #ffffff; border-radius: 12px; padding: 40px; text-align: center; border: 1px solid #e5e7eb;">

                <h2 style="color: #91a91c; margin-bottom: 20px;">
                    Bureau d'Ordre Digital
                </h2>

                <h3 style="color: #111827; font-size: 22px; margin-bottom: 15px;">
                    Vérification de votre adresse email
                </h3>

                <p style="color: #4b5563; font-size: 15px; line-height: 1.7;">
                    Bienvenue sur le Bureau d'Ordre Digital.
                    Pour activer votre compte, veuillez confirmer votre adresse email.
                </p>

                <a href="{{lien}}"
                   style="display: inline-block;
                          margin: 25px 0;
                          padding: 14px 30px;
                          background-color: #1e3a8a;
                          color: #ffffff;
                          text-decoration: none;
                          border-radius: 7px;
                          font-weight: bold;
                          font-size: 14px;">
                    Vérifier mon email
                </a>

                <p style="color: #6b7280; font-size: 13px; line-height: 1.6; margin-top: 25px;">
                    Ce lien est personnel et vous permet d'activer votre compte.
                </p>

                <p style="color: #9ca3af; font-size: 12px; line-height: 1.6; margin-top: 25px;">
                    Si vous n'êtes pas à l'origine de cette demande, vous pouvez ignorer cet email.
                </p>

                <hr style="border: 0; border-top: 1px solid #e5e7eb; margin: 30px 0;">

                <p style="color: #9ca3af; font-size: 11px;">
                    © 2026 Bureau d'Ordre Digital
                </p>

            </div>
        </div>
        """;
    }


    // =========================================================
    // RESET PASSWORD TEMPLATE
    // =========================================================

    private static string ConstruireTemplateResetPassword(
        string lien)
    {
        return $$"""
        <div style="
            font-family: Arial, sans-serif;
            background-color: #f4f6f8;
            padding: 40px 20px;
        ">

            <div style="
                max-width: 560px;
                margin: 0 auto;
                background-color: #ffffff;
                border-radius: 12px;
                padding: 40px;
                text-align: center;
                border: 1px solid #e5e7eb;
            ">

                <h2 style="
                    color: #91a91c;
                    margin-bottom: 20px;
                ">
                    Bureau d'Ordre Digital
                </h2>

                <h3 style="
                    color: #111827;
                    font-size: 22px;
                    margin-bottom: 15px;
                ">
                    Réinitialisation du mot de passe
                </h3>

                <p style="
                    color: #4b5563;
                    font-size: 15px;
                    line-height: 1.7;
                ">
                    Vous avez demandé la réinitialisation de votre mot de passe.
                </p>

                <p style="
                    color: #4b5563;
                    font-size: 15px;
                    line-height: 1.7;
                ">
                    Cliquez sur le bouton ci-dessous pour créer un nouveau mot de passe.
                </p>

                <a href="{{lien}}"
                   style="
                       display: inline-block;
                       margin: 25px 0;
                       padding: 14px 30px;
                       background-color: #1e3a8a;
                       color: #ffffff;
                       text-decoration: none;
                       border-radius: 7px;
                       font-weight: bold;
                       font-size: 14px;
                   ">
                    Réinitialiser mon mot de passe
                </a>

                <p style="
                    color: #6b7280;
                    font-size: 13px;
                    line-height: 1.6;
                ">
                    Ce lien est valable pendant 30 minutes.
                </p>

                <p style="
                    color: #9ca3af;
                    font-size: 12px;
                    line-height: 1.6;
                ">
                    Si vous n'avez pas demandé cette réinitialisation,
                    vous pouvez simplement ignorer cet email.
                </p>

                <hr style="
                    border: 0;
                    border-top: 1px solid #e5e7eb;
                    margin: 30px 0;
                ">

                <p style="
                    color: #9ca3af;
                    font-size: 11px;
                ">
                    © 2026 Bureau d'Ordre Digital
                </p>

            </div>

        </div>
        """;
    }
}