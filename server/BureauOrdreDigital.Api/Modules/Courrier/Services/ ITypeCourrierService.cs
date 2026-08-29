using BureauOrdreDigital.Api.Modules.Courrier.DTOs;

namespace BureauOrdreDigital.Api.Modules.Courrier.Services;

public interface ITypeCourrierService
{
    Task<List<TypeCourrierDto>> GetTousLesTypes();
}