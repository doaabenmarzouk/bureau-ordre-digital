using Microsoft.AspNetCore.Http;


namespace BureauOrdreDigital.Api.Modules.Courrier.Services;

public interface IStockageService
{
    Task<string> SauvegarderFichier(IFormFile fichier);
}