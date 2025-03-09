using System.Data.Common;
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
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new BaseResponse<string>("Erro inesperado", 500, "Ocorreu um erro no servidor. Tente novamente mais tarde."));
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
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new BaseResponse<string>("Erro inesperado", 500, "Ocorreu um erro no servidor. Tente novamente mais tarde."));
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
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new BaseResponse<string>("Erro inesperado", 500, "Ocorreu um erro no servidor. Tente novamente mais tarde."));
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
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new BaseResponse<string>("Erro inesperado", 500, "Ocorreu um erro no servidor. Tente novamente mais tarde."));
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
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new BaseResponse<string>("Erro inesperado", 500, "Ocorreu um erro no servidor. Tente novamente mais tarde."));
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
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new BaseResponse<string>("Erro inesperado", 500, "Ocorreu um erro no servidor. Tente novamente mais tarde."));
        }
    }
}