using System.Net.Http.Headers;
using System.Net.Http.Json;
using Iptv.Core.Handlers;
using Iptv.Core.Models;
using Iptv.Core.Requests.IdentityRequests;
using Iptv.Core.Responses;
using Iptv.Web.Services;

namespace Iptv.Web.Handlers;

public class IdentityHandler : IIdentityHandler
{
    private readonly IdentityServices _identityServices;
    private readonly HttpClient _httpClient;

    public IdentityHandler(IHttpClientFactory httpClientFactory, IdentityServices identityServices)
    {
        _identityServices = identityServices;
        _httpClient = httpClientFactory.CreateClient("identity");
    }
    
    public async Task<BaseResponse<string>> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response =  await _httpClient.PostAsJsonAsync("register", request);
            
            if (!response.IsSuccessStatusCode)
                await _identityServices.HandleRequestError(response);
            
            var baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
            
            if (baseResponse == null)
                return new BaseResponse<string>("Resposta inválida da API", 500, "A resposta da API não pôde ser processada.");
                
            return new BaseResponse<string>(baseResponse.Data ?? "Registro realizado com sucesso!" ,200, baseResponse.Message ?? "Registro realizado com sucesso!");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<string>("Falha na comunicação com o servidor", 500, ex.Message);
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>("Erro inesperado", 500, ex.Message);
        }
    }

    public async Task<BaseResponse<string>> LoginAsync(LoginRequest request)
    {
        try
        {
            var response =  await _httpClient.PostAsJsonAsync("login", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
                
                var errorMessage = errorResponse?.Message ?? "Falha no login";

                return new BaseResponse<string>(errorMessage, (int)response.StatusCode, errorMessage);
            }
            
            var baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
            
            if (baseResponse == null)
                return new BaseResponse<string>("Token inválido ou resposta inesperada", 500, "A resposta da API não contém um token válido.");

            var jwtToken = baseResponse.Data ?? "";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            return new BaseResponse<string>("Login realizado com sucesso!", 200, "Login realizado com sucesso!");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<string>("Falha na comunicação com o servidor", 500, ex.Message);
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>("Erro inesperado", 500, ex.Message);
        }
    }

    public async Task<BaseResponse<UserInfo>> UserInfo(string? userId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<UserInfo>>("user-info");

            if (response == null)
                return new BaseResponse<UserInfo>(null, 500, "Erro no servidor");
            
            return response.IsSuccess 
                ? new BaseResponse<UserInfo>(response.Data, 200, "Dados do usuario obtidos com sucesso!") 
                : new BaseResponse<UserInfo>(null, 500, "Erro ao obter os dados do usuario");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<UserInfo>(null, 500, ex.Message);
        }
        catch (Exception ex)
        {
            return new BaseResponse<UserInfo>(null, 500, ex.Message);
        }
    }

    public Task<BaseResponse<string>> UpdateUserInfo(UpdateUserInfoRequest updateUserInfo)
    {
        throw new NotImplementedException();
    }
}