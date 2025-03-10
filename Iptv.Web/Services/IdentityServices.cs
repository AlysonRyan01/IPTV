using System.Net.Http.Json;
using Iptv.Core.Responses;

namespace Iptv.Web.Services;

public class IdentityServices
{
    public async Task<BaseResponse<string>> HandleRequestError(HttpResponseMessage response)
    {
        try
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
            
            var errorMessage = errorResponse?.Message ?? "Falha no registro";

            return new BaseResponse<string>(errorMessage, (int)response.StatusCode, errorMessage);
        }
        catch
        {
            return new BaseResponse<string>("Erro ao obter a resposta da API", 500, "Erro ao obter a resposta da API");
        }
    }
}