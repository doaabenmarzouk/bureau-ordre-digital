using BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;
using BureauOrdreDigital.Api.Modules.Utilisateur.Models;

namespace BureauOrdreDigital.Api.Modules.Utilisateur.Services;

public interface IAuthService
{
    Task<Models.Utilisateur> Register(RegisterDto dto);

    Task<LoginResponseDto> Login(LoginDto dto);

    Task VerifyEmail(string token);
}