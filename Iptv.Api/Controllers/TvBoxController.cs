using System.Threading.Tasks;
using Iptv.Core.Handlers;
using Iptv.Core.Models;
using Iptv.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Iptv.Api.Controllers;

[ApiController]
[Route("v1/tvbox/")]
public class TvBoxController(ITvboxHandler tvboxHandler) : ControllerBase
{
    [HttpGet("get/{id}")]
    public async Task<ActionResult<TvBox>> GetTvBoxById([FromRoute]string id)
    {
        try
        {
            var result = await tvboxHandler.GetTvBoxById(id);
            
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }
        catch
        {
            return StatusCode(500, new BaseResponse<string>("Erro inesperado", 500, "Ocorreu um erro no servidor. Tente novamente mais tarde."));
        }
    }
}