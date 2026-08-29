using BureauOrdreDigital.Api.Modules.Courrier.Services;
using Microsoft.AspNetCore.Mvc;

namespace BureauOrdreDigital.Api.Modules.Courrier.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TypeCourrierController : ControllerBase
{
    private readonly ITypeCourrierService _typeCourrierService;

    public TypeCourrierController(ITypeCourrierService typeCourrierService)
    {
        _typeCourrierService = typeCourrierService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTousLesTypes()
    {
        var types = await _typeCourrierService.GetTousLesTypes();
        return Ok(types);
    }
}