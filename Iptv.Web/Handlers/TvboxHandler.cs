using System.Net.Http.Json;
using Iptv.Core.Handlers;
using Iptv.Core.Models;
using Iptv.Core.Responses;

namespace Iptv.Web.Handlers;

public class TvboxHandler : ITvboxHandler
{
    private readonly HttpClient _httpClient;
    
    public TvboxHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("tvbox");
    }
    
    public async Task<BaseResponse<TvBox>> GetTvBoxById(string id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<TvBox>>($"get/{id}");
            
            if (response == null)
                return new BaseResponse<TvBox>(null, 500, "Erro no servidor");
            
            return response.IsSuccess 
                ? new BaseResponse<TvBox>(response.Data, 200, "TvBox obtido com sucesso!") 
                : new BaseResponse<TvBox>(null, 500, "Erro ao obter o TvBox");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<TvBox>(null, 500, ex.Message);
        }
        catch (Exception ex)
        {
            return new BaseResponse<TvBox>(null, 500, ex.Message);
        }
    }
}