using System.Data.Common;
using System.Security.Claims;
using Iptv.Api.Services;
using Iptv.Core.Handlers;
using Iptv.Core.Requests.IdentityRequests;
using Iptv.Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iptv.Api.Controllers;

[ApiController]
[Route("v1/identity/")]
public class IdentityController(IIdentityHandler identityHandler, TokenService tokenService) : ControllerBase
{
    private readonly TokenService _tokenService = tokenService;

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
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Erro de argumento nulo: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Erro interno: dados necessários não foram fornecidos."
            ));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Erro de operação inválida: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Erro interno: operação não pode ser concluída."
            ));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro de comunicação: {ex}");
            return StatusCode(503, new BaseResponse<object>(
                null,
                503,
                "Serviço temporariamente indisponível. Tente novamente mais tarde."
            ));
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Erro de autorização: {ex}");
            return Unauthorized(new BaseResponse<object>(
                null,
                401,
                "Acesso negado. Você não tem permissão para acessar este recurso."
            ));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Ocorreu um erro inesperado. Por favor, contate o suporte técnico."
            ));
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

            if (string.IsNullOrEmpty(result.Data))
                return Unauthorized(result);

            _tokenService.SetTokensInsideCookie(result.Data, HttpContext);
            
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Erro de argumento nulo: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Erro interno: dados necessários não foram fornecidos."
            ));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Erro de operação inválida: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Erro interno: operação não pode ser concluída."
            ));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro de comunicação: {ex}");
            return StatusCode(503, new BaseResponse<object>(
                null,
                503,
                "Serviço temporariamente indisponível. Tente novamente mais tarde."
            ));
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Erro de autorização: {ex}");
            return Unauthorized(new BaseResponse<object>(
                null,
                401,
                "Acesso negado. Você não tem permissão para acessar este recurso."
            ));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Ocorreu um erro inesperado. Por favor, contate o suporte técnico."
            ));
        }
    }
    
    [Authorize]
    [HttpGet("user-info")]
    public async Task<IActionResult> UserInfo()
    {
        try
        {
            var user = User;

            if (user.Identity == null || !user.Identity.IsAuthenticated)
                return Unauthorized();
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new BaseResponse<string>("UserId não encontrado nas claims.", 401, "UserId não encontrado nas claims."));
            
            var result = await identityHandler.UserInfo(userId);
            
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Erro de argumento nulo: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Erro interno: dados necessários não foram fornecidos."
            ));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Erro de operação inválida: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Erro interno: operação não pode ser concluída."
            ));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro de comunicação: {ex}");
            return StatusCode(503, new BaseResponse<object>(
                null,
                503,
                "Serviço temporariamente indisponível. Tente novamente mais tarde."
            ));
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Erro de autorização: {ex}");
            return Unauthorized(new BaseResponse<object>(
                null,
                401,
                "Acesso negado. Você não tem permissão para acessar este recurso."
            ));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Ocorreu um erro inesperado. Por favor, contate o suporte técnico."
            ));
        }
    }

    [Authorize]
    [HttpPut("update-user")]
    public async Task<IActionResult> UpdateUserAsync(UpdateUserInfoRequest request)
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
            var user = User;

            if (user.Identity == null || !user.Identity.IsAuthenticated)
                return Unauthorized();
            
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            
            var result = await identityHandler.UpdateUserInfo(request);
            
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Erro de argumento nulo: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Erro interno: dados necessários não foram fornecidos."
            ));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Erro de operação inválida: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Erro interno: operação não pode ser concluída."
            ));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro de comunicação: {ex}");
            return StatusCode(503, new BaseResponse<object>(
                null,
                503,
                "Serviço temporariamente indisponível. Tente novamente mais tarde."
            ));
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Erro de autorização: {ex}");
            return Unauthorized(new BaseResponse<object>(
                null,
                401,
                "Acesso negado. Você não tem permissão para acessar este recurso."
            ));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex}");
            return StatusCode(500, new BaseResponse<object>(
                null,
                500,
                "Ocorreu um erro inesperado. Por favor, contate o suporte técnico."
            ));
        }
    }
}