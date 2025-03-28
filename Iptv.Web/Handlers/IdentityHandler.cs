using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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
            var response = await _httpClient.PostAsJsonAsync("register", request);
            
            if (!response.IsSuccessStatusCode)
                await _identityServices.HandleRequestError(response);
            
            var baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
            
            if (baseResponse == null)
                return new BaseResponse<string>("Resposta inválida da API", 500, "A resposta da API não pôde ser processada.");
                
            return new BaseResponse<string>(baseResponse.Data ?? "Registro realizado com sucesso!", 200, baseResponse.Message ?? "Registro realizado com sucesso!");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new BaseResponse<string>("Serviço não encontrado", 404, "O serviço de registro não está disponível");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new BaseResponse<string>("Não autorizado", 401, "Credenciais inválidas para acessar o serviço");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return new BaseResponse<string>("Requisição inválida", 400, "Dados de registro incorretos");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<string>("Falha na comunicação", 503, $"Erro no serviço de registro: {ex.Message}");
        }
        catch (JsonException)
        {
            return new BaseResponse<string>("Formato inválido", 500, "Resposta do servidor em formato incorreto");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new BaseResponse<string>("Tempo esgotado", 504, "Tempo de conexão com o serviço expirado");
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>("Erro inesperado", 500, $"Falha no registro: {ex.Message}");
        }
    }

    public async Task<BaseResponse<string>> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("login", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
                var errorMessage = errorResponse?.Message ?? "Falha no login";
                return new BaseResponse<string>(errorMessage, (int)response.StatusCode, errorMessage);
            }
            
            var baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
            
            if (baseResponse == null)
                return new BaseResponse<string>("Token inválido", 500, "Resposta de login inválida");

            var jwtToken = baseResponse.Data ?? "";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            return new BaseResponse<string>("Login realizado", 200, "Autenticação bem-sucedida");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new BaseResponse<string>("Credenciais inválidas", 401, "Usuário ou senha incorretos");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
        {
            return new BaseResponse<string>("Muitas tentativas", 429, "Muitas tentativas de login. Tente novamente mais tarde");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<string>("Serviço indisponível", 503, $"Serviço de autenticação offline: {ex.Message}");
        }
        catch (JsonException)
        {
            return new BaseResponse<string>("Formato inválido", 500, "Resposta de login em formato inesperado");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new BaseResponse<string>("Tempo esgotado", 504, "Serviço de autenticação não respondeu a tempo");
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>("Falha no login", 500, $"Erro durante o login: {ex.Message}");
        }
    }

    public async Task<BaseResponse<UserInfo>> UserInfo(string? userId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<UserInfo>>("user-info");

            if (response == null)
                return new BaseResponse<UserInfo>(null, 500, "Resposta vazia do servidor");
            
            return response.IsSuccess 
                ? new BaseResponse<UserInfo>(response.Data, 200, "Informações obtidas") 
                : new BaseResponse<UserInfo>(null, 500, response.Message ?? "Erro ao buscar informações");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new BaseResponse<UserInfo>(null, 404, "Serviço de informações não encontrado");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
        {
            return new BaseResponse<UserInfo>(null, 403, "Acesso negado às informações do usuário");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<UserInfo>(null, 503, $"Serviço de informações indisponível: {ex.Message}");
        }
        catch (JsonException)
        {
            return new BaseResponse<UserInfo>(null, 500, "Formato de dados inválido");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new BaseResponse<UserInfo>(null, 504, "Tempo de resposta excedido");
        }
        catch (Exception ex)
        {
            return new BaseResponse<UserInfo>(null, 500, $"Erro ao carregar informações: {ex.Message}");
        }
    }

    public async Task<BaseResponse<string>> UpdateUserInfo(UpdateUserInfoRequest updateUserInfo)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("user-info", updateUserInfo);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();
                var errorMessage = errorResponse?.Message ?? response.ReasonPhrase ?? "Falha na atualização";
                return new BaseResponse<string>(errorMessage, (int)response.StatusCode, errorMessage);
            }
            
            return new BaseResponse<string>("Atualizado com sucesso", 200, "Dados atualizados");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return new BaseResponse<string>("Dados inválidos", 400, "Verifique os dados enviados");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
            return new BaseResponse<string>("Conflito", 409, "Os dados já estão em uso");
        }
        catch (HttpRequestException ex)
        {
            return new BaseResponse<string>("Serviço indisponível", 503, $"Erro na atualização: {ex.Message}");
        }
        catch (JsonException)
        {
            return new BaseResponse<string>("Formato inválido", 500, "Erro no processamento dos dados");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new BaseResponse<string>("Tempo esgotado", 504, "Serviço não respondeu");
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>("Erro inesperado", 500, $"Falha na atualização: {ex.Message}");
        }
    }
}