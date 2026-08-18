using BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;

namespace BureauOrdreDigital.Api.Modules.Utilisateur.Services;

public interface IUtilisateurService
{
    Task<UtilisateurResponseDto> GetProfil(Guid id);
    Task<UtilisateurResponseDto> UpdateProfil(Guid id, ProfilUpdateDto dto);
}