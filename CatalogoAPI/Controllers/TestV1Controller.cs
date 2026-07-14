using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPI.Controllers;

[ApiController]
[ApiVersion("1.0", Deprecated = true)]
[Route("api/v{version:apiVersion}/test")]
[ApiExplorerSettings(IgnoreApi = true)]
public class TestV1Controller : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Resposta V1.0 - Teste de Roteamento");
    }
}