using System.Security.Claims;
using Iptv.Core.Handlers;
using Iptv.Core.Models;
using Iptv.Core.Requests.OrderRequests;
using Iptv.Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iptv.Api.Controllers;

[ApiController]
[Authorize]
[Route("v1/order/")]
public class OrderController(IOrderHandler orderHandler) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult<Order>> CreateOrderAsync(CreateOrderRequest request)
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("ID do usuário inválido.");
            }
            
            request.UserId = userId;
            
            var result = await orderHandler.CreateAsync(request);
            
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
    
    [HttpPost("pay")]
    public async Task<ActionResult<Order>> PayOrderAsync(PayOrderRequest request)
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("ID do usuário inválido.");
            }
            
            request.UserId = userId;
            
            var result = await orderHandler.PayAsync(request);
            
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
    
    [HttpPost("cancel")]
    public async Task<ActionResult<Order>> CancelOrderAsync(CancelOrderRequest request)
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("ID do usuário inválido.");
            }
            
            request.UserId = userId;
            
            var result = await orderHandler.CancelAsync(request);
            
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
    
    [HttpPost("refund")]
    public async Task<ActionResult<Order>> RefundOrderAsync(RefundOrderRequest request)
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("ID do usuário inválido.");
            }
            
            request.UserId = userId;
            
            var result = await orderHandler.RefundAsync(request);
            
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
    
    [HttpPost("GetAll")]
    public async Task<ActionResult<Order>> GetAllOrdersAsync(GetAllOrdersRequest request)
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("ID do usuário inválido.");
            }
            
            request.UserId = userId;
            
            var result = await orderHandler.GetAllAsync(request);
            
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
    
    [HttpPost("GetByNumber")]
    public async Task<ActionResult<Order>> GetOrderByNumberAsync(GetOrderByNumberRequest request)
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("ID do usuário inválido.");
            }
            
            request.UserId = userId;
            
            var result = await orderHandler.GetByNumberAsync(request);
            
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