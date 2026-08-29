using BureauOrdreDigital.Api.Modules.Courrier.Services;
using Microsoft.AspNetCore.Mvc;

namespace BureauOrdreDigital.Api.Modules.Courrier.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceDestinataireController : ControllerBase
{
    private readonly IServiceDestinataireService _serviceDestinataireService;

    public ServiceDestinataireController(IServiceDestinataireService serviceDestinataireService)
    {
        _serviceDestinataireService = serviceDestinataireService;
    }

    [HttpGet]
    public async Task<IActionResult> RechercherServices(
        [FromQuery] string? ville,
        [FromQuery] string? type,
        [FromQuery] string? motCle)
    {
        var services = await _serviceDestinataireService.RechercherServices(ville, type, motCle);
        return Ok(services);
    }
}