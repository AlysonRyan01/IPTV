using Iptv.Core.Requests.MelhorEnvio;
using Iptv.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iptv.Api.Controllers;

[ApiController]
[Route("v1/melhor-envio")]
public class MelhorEnvioController(IMelhorEnvioService service) : ControllerBase
{
    [HttpPost("frete")]
    public async Task<ActionResult> CalcularFreteAsync(CalcularFreteRequest request)
    {
        try
        {
            var response = await service.CalcularFreteAsync(request);
            
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch 
        {
            return StatusCode(500, "Internal server error");
        }
    }
}