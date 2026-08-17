namespace BureauOrdreDigital.Api.Shared.Services;

public class EmailService
{
    public void EnvoyerEmailVerification(string email, string token)
    {
        var lien = $"https://localhost:5001/api/auth/verify-email?token={token}";

        Console.WriteLine("=== EMAIL DE VÉRIFICATION (stub) ===");
        Console.WriteLine($"À : {email}");
        Console.WriteLine($"Lien de vérification : {lien}");
        Console.WriteLine("=====================================");
    }
}