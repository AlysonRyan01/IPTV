using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Iptv.Core.Handlers;
using Iptv.Core.Models;
using Iptv.Core.Responses;

namespace Iptv.Web.Handlers;

public class AddressHandler : IAddressHandler
{
    private readonly HttpClient _client;

    public AddressHandler(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("address");
    }
    
    public async Task<BaseResponse<Address>> GetAddress(string? userId)
    {
        try
        {
            var response = await _client.GetFromJsonAsync<BaseResponse<Address>>($"get");
            
            if (response == null)
                return new BaseResponse<Address>(null, 500, "Resposta inválida do servidor de endereços");
            
            return response.IsSuccess 
                ? new BaseResponse<Address>(response.Data, 200, "Endereço obtido com sucesso") 
                : new BaseResponse<Address>(null, 500, response.Message ?? "Erro ao obter o endereço");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new BaseResponse<Address>(null, 404, "Serviço de endereços não encontrado");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new BaseResponse<Address>(null, 401, "Autenticação falhou no serviço de endereços");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<Address>(null, 503, $"Serviço de endereços indisponível: {ex.Message}");
        }
        catch (JsonException)
        {
            return new BaseResponse<Address>(null, 500, "Erro ao processar a resposta do servidor de endereços");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new BaseResponse<Address>(null, 504, "Tempo de conexão com o serviço de endereços expirado");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex}");
            return new BaseResponse<Address>(null, 500, "Erro interno ao acessar o serviço de endereços");
        }
    }

    public async Task<BaseResponse<string>> UpdateAddress(string? userId, Address address)
    {
        try
        {
            var response = await _client.PutAsJsonAsync($"update", address);
            
            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
                    return new BaseResponse<string>(
                        null,
                        (int)response.StatusCode,
                        errorResponse?.Message ?? $"Falha ao atualizar: {response.ReasonPhrase}"
                    );
                }
                catch (JsonException)
                {
                    return new BaseResponse<string>(
                        null,
                        (int)response.StatusCode,
                        $"Falha ao atualizar: {response.ReasonPhrase}"
                    );
                }
            }
            
            return new BaseResponse<string>(
                "Endereço atualizado com sucesso", 
                200, 
                "Endereço atualizado com sucesso"
            );
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new BaseResponse<string>(null, 404, "Serviço de endereços não encontrado");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new BaseResponse<string>(null, 401, "Autenticação falhou no serviço de endereços");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<string>(null, 503, $"Serviço de endereços indisponível: {ex.Message}");
        }
        catch (JsonException)
        {
            return new BaseResponse<string>(null, 500, "Erro ao processar os dados do endereço");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new BaseResponse<string>(null, 504, "Tempo de conexão com o serviço de endereços expirado");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex}");
            return new BaseResponse<string>(null, 500, "Erro interno ao atualizar o endereço");
        }
    }
}