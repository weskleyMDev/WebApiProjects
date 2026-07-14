namespace CatalogoAPI.Controllers;

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/test")]
[ApiExplorerSettings(IgnoreApi = true)]
public class TestV2Controller :  ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Resposta V2.0 - Teste de Roteamento");
    }
}