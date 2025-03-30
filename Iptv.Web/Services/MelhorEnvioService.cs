using System.Net.Http.Json;
using Iptv.Core.Requests.MelhorEnvio;
using Iptv.Core.Responses;
using Iptv.Core.Responses.MelhorEnvio;
using Iptv.Core.Services;

namespace Iptv.Web.Services;

public class MelhorEnvioService : IMelhorEnvioService
{
    private readonly HttpClient _client;

    public MelhorEnvioService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("melhorenvio");
    }
    
    public async Task<BaseResponse<List<CalculoFreteResponse>>> CalcularFreteAsync(CalcularFreteRequest request)
    {
        try
        {
            var result = await _client.PostAsJsonAsync("frete", request);
            
            if (!result.IsSuccessStatusCode)
                return new BaseResponse<List<CalculoFreteResponse>>(null, 500, "Erro no servidor");
            
            var response = await result.Content.ReadFromJsonAsync<MelhorEnvioResponse>();
                
            if (response!.Data.Count == 0)
                return new BaseResponse<List<CalculoFreteResponse>>(null, 500, "Erro no servidor");

            var fretes = response.Data;
            
            return fretes.Any()
                ? new BaseResponse<List<CalculoFreteResponse>>(fretes, 200, "Fretes obtidos com sucesso!") 
                : new BaseResponse<List<CalculoFreteResponse>>(null, 500, "Erro ao obter os fretes!");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<List<CalculoFreteResponse>>(null, 500, ex.Message);
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<CalculoFreteResponse>>(null, 500, ex.Message);
        }
    }
}