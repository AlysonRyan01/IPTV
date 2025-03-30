using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Iptv.Core.Handlers;
using Iptv.Core.Models;
using Iptv.Core.Requests.OrderRequests;
using Iptv.Core.Responses;

namespace Iptv.Web.Handlers;

public class OrderHandler : IOrderHandler
{
    private readonly HttpClient _client;

    public OrderHandler(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("order");
    }
    public async Task<BaseResponse<Order?>> CancelAsync(CancelOrderRequest request)
    {
        try
        {
            var result = await _client.PostAsJsonAsync($"cancel/{request.Id}", request);
            return await result.Content.ReadFromJsonAsync<BaseResponse<Order?>>()
                   ?? new BaseResponse<Order?>(null, 400, "Falha ao cancelar pedido");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new BaseResponse<Order?>(null, 404, "O serviço de pedido não está disponível");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new BaseResponse<Order?>(null, 401, "Credenciais inválidas para acessar o pedido");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return new BaseResponse<Order?>(null, 400, "Dados de pedido incorretos");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<Order?>(null, 503, $"Erro no serviço de pedido: {ex.Message}");
        }
        catch (JsonException)
        {
            return new BaseResponse<Order?>(null, 500, "Resposta do servidor em formato incorreto");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new BaseResponse<Order?>(null, 504, "Tempo de conexão com o serviço expirado");
        }
        catch (Exception ex)
        {
            return new BaseResponse<Order?>(null, 500, $"Falha no pedido: {ex.Message}");
        }
    }

    public async Task<BaseResponse<Order?>> CreateAsync(CreateOrderRequest request)
    {
        try
        {
            var result = await _client.PostAsJsonAsync("create", request);
            return await result.Content.ReadFromJsonAsync<BaseResponse<Order?>>()
                   ?? new BaseResponse<Order?>(null, 400, "Falha ao criar a pedido");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new BaseResponse<Order?>(null, 404, "O serviço de pedido não está disponível");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new BaseResponse<Order?>(null, 401, "Credenciais inválidas para acessar o pedido");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return new BaseResponse<Order?>(null, 400, "Dados de pedido incorretos");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<Order?>(null, 503, $"Erro no serviço de pedido: {ex.Message}");
        }
        catch (JsonException)
        {
            return new BaseResponse<Order?>(null, 500, "Resposta do servidor em formato incorreto");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new BaseResponse<Order?>(null, 504, "Tempo de conexão com o serviço expirado");
        }
        catch (Exception ex)
        {
            return new BaseResponse<Order?>(null, 500, $"Falha no pedido: {ex.Message}");
        }
    }

    public async Task<BaseResponse<Order?>> PayAsync(PayOrderRequest request)
    {
        try
        {
            var result = await _client.PostAsJsonAsync("pay", request);
            return await result.Content.ReadFromJsonAsync<BaseResponse<Order?>>()
                   ?? new BaseResponse<Order?>(null, 400, "Falha ao pagar pedido");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new BaseResponse<Order?>(null, 404, "O serviço de pedido não está disponível");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new BaseResponse<Order?>(null, 401, "Credenciais inválidas para acessar o pedido");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return new BaseResponse<Order?>(null, 400, "Dados de pedido incorretos");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<Order?>(null, 503, $"Erro no serviço de pedido: {ex.Message}");
        }
        catch (JsonException)
        {
            return new BaseResponse<Order?>(null, 500, "Resposta do servidor em formato incorreto");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new BaseResponse<Order?>(null, 504, "Tempo de conexão com o serviço expirado");
        }
        catch (Exception ex)
        {
            return new BaseResponse<Order?>(null, 500, $"Falha no pedido: {ex.Message}");
        }
    }

    public async Task<BaseResponse<Order?>> RefundAsync(RefundOrderRequest request)
    {
        try
        {
            var result = await _client.PostAsJsonAsync("refund", request);
            return await result.Content.ReadFromJsonAsync<BaseResponse<Order?>>()
                   ?? new BaseResponse<Order?>(null, 400, "Falha ao reembolsar o pedido");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new BaseResponse<Order?>(null, 404, "O serviço de pedido não está disponível");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new BaseResponse<Order?>(null, 401, "Credenciais inválidas para acessar o pedido");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return new BaseResponse<Order?>(null, 400, "Dados de pedido incorretos");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<Order?>(null, 503, $"Erro no serviço de pedido: {ex.Message}");
        }
        catch (JsonException)
        {
            return new BaseResponse<Order?>(null, 500, "Resposta do servidor em formato incorreto");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new BaseResponse<Order?>(null, 504, "Tempo de conexão com o serviço expirado");
        }
        catch (Exception ex)
        {
            return new BaseResponse<Order?>(null, 500, $"Falha no pedido: {ex.Message}");
        }
    }

    public async Task<BaseResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
    {
        try
        {
            var result = await _client.GetFromJsonAsync<BaseResponse<List<Order>?>>($"GetAll/{request.UserId}");
            
            if (result == null || !result.Data!.Any())
                return new BaseResponse<List<Order>?>(null, 400, "Falha ao obter os pedidos");
            
            return result.IsSuccess ? result : new BaseResponse<List<Order>?>(null, 500, "Falha no pedido");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new BaseResponse<List<Order>?>(null, 404, "O serviço de pedido não está disponível");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new BaseResponse<List<Order>?>(null, 401, "Credenciais inválidas para acessar o pedido");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return new BaseResponse<List<Order>?>(null, 400, "Dados de pedido incorretos");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<List<Order>?>(null, 503, $"Erro no serviço de pedido: {ex.Message}");
        }
        catch (JsonException)
        {
            return new BaseResponse<List<Order>?>(null, 500, "Resposta do servidor em formato incorreto");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new BaseResponse<List<Order>?>(null, 504, "Tempo de conexão com o serviço expirado");
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<Order>?>(null, 500, $"Falha no pedido: {ex.Message}");
        }
    }

    public async Task<BaseResponse<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
    {
        try
        {
            var result = await _client.GetFromJsonAsync<BaseResponse<Order?>>($"GetByNumber/{request.Number}");
            
            if (result == null)
                return new BaseResponse<Order?>(null, 400, "Falha ao obter o pedido");
            
            return result.IsSuccess ? result : new BaseResponse<Order?>(null, 500, "Falha no pedido");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new BaseResponse<Order?>(null, 404, "O serviço de pedido não está disponível");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new BaseResponse<Order?>(null, 401, "Credenciais inválidas para acessar o pedido");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return new BaseResponse<Order?>(null, 400, "Dados de pedido incorretos");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<Order?>(null, 503, $"Erro no serviço de pedido: {ex.Message}");
        }
        catch (JsonException)
        {
            return new BaseResponse<Order?>(null, 500, "Resposta do servidor em formato incorreto");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new BaseResponse<Order?>(null, 504, "Tempo de conexão com o serviço expirado");
        }
        catch (Exception ex)
        {
            return new BaseResponse<Order?>(null, 500, $"Falha no pedido: {ex.Message}");
        }
    }
}