using System.Data.Common;
using Iptv.Core.Handlers;
using Iptv.Core.Requests.IdentityRequests;
using Iptv.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Iptv.Api.Controllers;

[ApiController]
[Route("v1/identity/")]
public class IdentityController(IIdentityHandler identityHandler) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            
            return BadRequest(new BaseResponse<string>("Erro de validação", 400, string.Join(", ", errors)));
        }
        
        try
        {
            var result = await identityHandler.RegisterAsync(model);
            
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }
        catch (DbException ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new BaseResponse<string>("Erro no banco de dados", 500, ex.Message));
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex);
            return BadRequest(new BaseResponse<string>("Argumento inválido", 400, ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex);
            return BadRequest(new BaseResponse<string>("Erro de operação", 400, ex.Message));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new BaseResponse<string>("Erro inesperado", 500, "Ocorreu um erro no servidor. Tente novamente mais tarde."));
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            
            return BadRequest(new BaseResponse<string>("Erro de validação", 400, string.Join(", ", errors)));
        }
        
        try
        {
            var result = await identityHandler.LoginAsync(model);
            
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }
        catch (DbException ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new BaseResponse<string>("Erro no banco de dados", 500, ex.Message));
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex);
            return BadRequest(new BaseResponse<string>("Argumento inválido", 400, ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex);
            return BadRequest(new BaseResponse<string>("Erro de operação", 400, ex.Message));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new BaseResponse<string>("Erro inesperado", 500, "Ocorreu um erro no servidor. Tente novamente mais tarde."));
        }
    }
}