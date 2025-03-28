using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using Iptv.Core.Handlers;
using Iptv.Core.Models;
using Iptv.Core.Requests.IdentityRequests;
using Iptv.Core.Requests.MelhorEnvio;
using Iptv.Core.Responses.MelhorEnvio;
using Iptv.Core.Services;
using Iptv.Web.Authentication;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Iptv.Web.Pages;

public partial class Checkout : ComponentBase
{
    
    private UserInfo UserInfo { get; set; } = new();
    private UpdateUserInfoRequest UpdateUser { get; set; } = new();
    private Address Address { get; set; } = new();
    private TvBox TvBox { get; set; } = new();
    private List<CalculoFreteResponse> Fretes { get; set; } = new();
    
    private bool PageIsBusy { get; set; }
    
    private bool FuncValidateUserIsBusy { get; set; }
    private bool UserValidated { get; set; }
    
    private bool FuncValidateAddressIsBusy { get; set; }
    private bool AddressValidated { get; set; }
    
    private bool FuncCalcularFreteIsBusy { get; set; }
    private bool FreteValidated { get; set; }
    
    [SupplyParameterFromQuery(Name = "product")]
    public string ProductId { get; set; } = null!;
    
    [SupplyParameterFromQuery(Name = "quantity")]
    public string Quantity { get; set; } = null!;
    
    

    [Inject] private IIdentityHandler IdentityHandler { get; set; } = null!;
    [Inject] private ITvboxHandler TvboxHandler { get; set; } = null!;
    [Inject] private IMelhorEnvioService MelhorEnvioService { get; set; } = null!;
    [Inject] private IAddressHandler AddressHandler { get; set; } = null!;
    [Inject] private AuthStateProvider AuthStateProvider { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    
    
    protected override async Task OnInitializedAsync()
    {
        PageIsBusy = true;
        try
        {
            if (string.IsNullOrEmpty(ProductId))
            {
                Snackbar.Add("Produto não especificado", Severity.Error);
                NavigationManager.NavigateTo("/");
                return;
            }
            
            var auth = await AuthStateProvider.GetAuthenticationStateAsync();

            var user = auth.User;

            if (user.Identity?.IsAuthenticated == false)
            {
                NavigationManager.NavigateTo("/");
            }

            var userInfoResult = await IdentityHandler.UserInfo(null);

            if (userInfoResult.IsSuccess)
                UserInfo = userInfoResult.Data ?? new();
            else
                Snackbar.Add($"Error: {userInfoResult.Message}", Severity.Error);
            
            var addressResult = await AddressHandler.GetAddress(null);
            
            if (addressResult.IsSuccess)
                Address = addressResult.Data ?? new();
            else
                Snackbar.Add($"Error: {addressResult.Message}", Severity.Error);

            var tvboxResult = await TvboxHandler.GetTvBoxById(ProductId);

            if (tvboxResult.IsSuccess)
                TvBox = tvboxResult.Data ?? new();
            else
                Snackbar.Add($"Error: {tvboxResult.Message}", Severity.Error);

        }
        catch (HttpRequestException httpEx) when (httpEx.StatusCode == HttpStatusCode.Unauthorized)
        {
            Snackbar.Add("Sessão expirada. Por favor, faça login novamente.", Severity.Error);
            NavigationManager.NavigateTo("/logout");
        }
        catch (HttpRequestException httpEx) when (httpEx.StatusCode == HttpStatusCode.NotFound)
        {
            Snackbar.Add("Produto não encontrado.", Severity.Error);
            NavigationManager.NavigateTo("/produtos");
        }
        catch (HttpRequestException)
        {
            Snackbar.Add("Erro de rede: Falha na comunicação com o servidor", Severity.Error);
        }
        catch (AuthenticationException)
        {
            Snackbar.Add("Falha na autenticação. Por favor, faça login novamente.", Severity.Error);
            NavigationManager.NavigateTo("/logout");
        }
        catch (InvalidOperationException invOpEx)
        {
            Snackbar.Add($"Operation error: {invOpEx.Message}", Severity.Error);
        }
        catch (NullReferenceException)
        {
            Snackbar.Add("Ocorreu um erro no sistema. Tente novamente mais tarde.", Severity.Error);
        }
        catch (Exception)
        {
            Snackbar.Add("Ocorreu um erro no sistema. Tente novamente mais tarde.", Severity.Error);
        }
        finally
        {
            PageIsBusy = false;
        }
    }
    
    

    private async Task ValidateUser()
    {
        FuncValidateUserIsBusy = true;
        try
        {
            UpdateUser.Address = UserInfo.Address;
            UpdateUser.FirstName = UserInfo.FirstName;
            UpdateUser.LastName = UserInfo.LastName;
            UpdateUser.Email = UserInfo.Email;
            UpdateUser.Phone = UserInfo.Phone;
            
            if (string.IsNullOrEmpty(UpdateUser.FirstName))
            {
                Snackbar.Add("Nome é obrigatório", Severity.Warning);
                return;
            }

            var updateUserResult = await IdentityHandler.UpdateUserInfo(UpdateUser);

            if (updateUserResult.IsSuccess)
            {
                Snackbar.Add("Dados validados com sucesso!", Severity.Success);
                UserValidated = true;
            }
            else
            {
                Snackbar.Add($"Falha na validação: {updateUserResult.Message}", Severity.Error);
            }
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            Snackbar.Add("Dados do usuario inválidos. Verifique os campos preenchidos.", Severity.Warning);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            Snackbar.Add("Sessão expirada. Por favor, faça login novamente.", Severity.Error);
            NavigationManager.NavigateTo("/logout");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            Snackbar.Add("Serviço de usuario indisponível no momento.", Severity.Error);
        }
        catch (HttpRequestException ex)
        {
            Snackbar.Add($"Falha na comunicação com o servidor: {ex.Message}", Severity.Error);
        }
        catch (JsonException)
        {
            Snackbar.Add("Erro no processamento dos dados do usuario.", Severity.Error);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            Snackbar.Add("Tempo de conexão esgotado. Tente novamente.", Severity.Warning);
        }
        catch (InvalidOperationException ex)
        {
            Snackbar.Add($"Operação inválida: {ex.Message}", Severity.Error);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro: {e.Message}", Severity.Error);
        }
        finally
        {
            FuncValidateUserIsBusy = false;
        }
    }

    private async Task ValidateAddress()
    {
        FuncValidateAddressIsBusy = true;
        try
        {
            var addressResult = await AddressHandler.UpdateAddress(null, Address);

            if (addressResult.IsSuccess)
            {
                Snackbar.Add("Dados validados com sucesso!", Severity.Success);
                AddressValidated = true;
                await CalcularFrete();
            }
            else
            {
                Snackbar.Add($"Ocorreu um erro: {addressResult.Message}", Severity.Error);
            }
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            Snackbar.Add("Dados do endereço inválidos. Verifique os campos preenchidos.", Severity.Warning);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            Snackbar.Add("Sessão expirada. Por favor, faça login novamente.", Severity.Error);
            NavigationManager.NavigateTo("/logout");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            Snackbar.Add("Serviço de endereço indisponível no momento.", Severity.Error);
        }
        catch (HttpRequestException ex)
        {
            Snackbar.Add($"Falha na comunicação com o servidor: {ex.Message}", Severity.Error);
        }
        catch (JsonException)
        {
            Snackbar.Add("Erro no processamento dos dados do endereço.", Severity.Error);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            Snackbar.Add("Tempo de conexão esgotado. Tente novamente.", Severity.Warning);
        }
        catch (InvalidOperationException ex)
        {
            Snackbar.Add($"Operação inválida: {ex.Message}", Severity.Error);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro: {e.Message}", Severity.Error);
        }
        finally
        {
            FuncValidateAddressIsBusy = false;
        }
    }

    public async Task CalcularFrete()
    {
        FuncCalcularFreteIsBusy = true;
        try
        {
            var request = new CalcularFreteRequest
            {
                CepDestino = Address.ZipCode
            };

            var freteResult = await MelhorEnvioService.CalcularFreteAsync(request);

            if (freteResult.IsSuccess)
            {
                Fretes = freteResult.Data ?? new();
                FreteValidated = true;
            }
            else
                Snackbar.Add("Ocorreu um erro ao calcular o frete.", Severity.Error);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            Snackbar.Add("Dados do frete inválidos. Verifique os campos preenchidos.", Severity.Warning);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            Snackbar.Add("Sessão expirada. Por favor, faça login novamente.", Severity.Error);
            NavigationManager.NavigateTo("/logout");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            Snackbar.Add("Serviço de frete indisponível no momento.", Severity.Error);
        }
        catch (HttpRequestException ex)
        {
            Snackbar.Add($"Falha na comunicação com o servidor: {ex.Message}", Severity.Error);
        }
        catch (JsonException)
        {
            Snackbar.Add("Erro no processamento dos dados do frete.", Severity.Error);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            Snackbar.Add("Tempo de conexão esgotado. Tente novamente.", Severity.Warning);
        }
        catch (InvalidOperationException ex)
        {
            Snackbar.Add($"Operação inválida: {ex.Message}", Severity.Error);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro: {e.Message}", Severity.Error);
        }
        finally
        {
            FuncCalcularFreteIsBusy = false;
        }
    }
}