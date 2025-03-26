using System.Net.Http.Headers;
using Iptv.Core.Requests.MelhorEnvio;
using Iptv.Core.Responses;
using Iptv.Core.Responses.MelhorEnvio;
using Iptv.Core.Services;

namespace Iptv.Api.Services;

public class MelhorEnvioService : IMelhorEnvioService
{
    private readonly HttpClient _httpClient;
    
    public MelhorEnvioService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            ApiConfiguration.MelhorEnvioToken);
        
        _httpClient.BaseAddress = new Uri("https://sandbox.melhorenvio.com.br/api/v2/");
    }
    
    public async Task<BaseResponse<List<CalculoFreteResponse>>> CalcularFreteAsync(CalcularFreteRequest request)
    {
        try
        {
            var payload = new
            {
                from = new { postal_code = request.CepOrigem },
                to = new { postal_code = request.CepDestino },
                package = new
                {
                    weight = request.Peso,
                    height = request.Altura,
                    width = request.Largura,
                    length = request.Comprimento
                }
            };
            
            var response = await _httpClient.PostAsJsonAsync("me/shipment/calculate", payload);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<CalculoFreteResponse>>();

                if (result == null || !result.Any())
                {
                    return new BaseResponse<List<CalculoFreteResponse>>(null, 500, "Nenhum serviço disponível.");
                }
                
                
                return new BaseResponse<List<CalculoFreteResponse>>(result, 200, "Frete calculado com sucesso.");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new BaseResponse<List<CalculoFreteResponse>>(null, 500, $"Erro: {error}");
            }
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<List<CalculoFreteResponse>>(null, 500, $"Erro ao calcular o frete. {ex.Message} ");
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<CalculoFreteResponse>>(null, 500, $"Erro ao calcular o frete. {ex.Message} ");
        }
    }
}