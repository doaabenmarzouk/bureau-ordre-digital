using BureauOrdreDigital.Api.Modules.Courrier.Services;
using Microsoft.AspNetCore.Mvc;

namespace BureauOrdreDigital.Api.Modules.Courrier.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganismeController : ControllerBase
{
    private readonly IOrganismeService _organismeService;

    public OrganismeController(IOrganismeService organismeService)
    {
        _organismeService = organismeService;
    }

    [HttpGet]
    public async Task<IActionResult> RechercherOrganismes(
        [FromQuery] string? ville,
        [FromQuery] string? type,
        [FromQuery] string? motCle)
    {
        var organismes = await _organismeService
            .RechercherOrganismes(ville, type, motCle);

        return Ok(organismes);
    }
}