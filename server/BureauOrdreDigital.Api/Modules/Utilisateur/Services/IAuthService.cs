using BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;
using BureauOrdreDigital.Api.Modules.Utilisateur.Models;

namespace BureauOrdreDigital.Api.Modules.Utilisateur.Services;

public interface IAuthService
{
    Task<Models.Utilisateur> Register(RegisterDto dto);

    Task<LoginResponseDto> Login(LoginDto dto);

    Task<LoginResponseDto> VerifyOtp(VerifyOtpDto dto);

    Task VerifyEmail(string token);

    Task ForgotPassword(ForgotPasswordDto dto);

    Task ResetPassword(ResetPasswordDto dto);
}