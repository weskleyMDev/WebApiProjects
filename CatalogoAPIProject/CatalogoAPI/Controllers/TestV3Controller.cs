using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPI.Controllers;

[ApiController]
[ApiVersion(3)]
[ApiVersion(4)]
[Route("api/test")]
[ApiExplorerSettings(IgnoreApi = true)]
public class TestV3Controller : ControllerBase
{
    [HttpGet]
    [MapToApiVersion(3)]
    public IActionResult GetV3()
    {
        return Ok("Resposta V3.0 - Teste de Roteamento");
    }

    [HttpGet]
    [MapToApiVersion(4)]
    public IActionResult GetV4()
    {
        return Ok("Resposta V4.0 - Teste de Roteamento");
    }
}