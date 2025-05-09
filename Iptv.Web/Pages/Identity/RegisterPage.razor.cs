using Iptv.Core.Handlers;
using Iptv.Core.Requests.IdentityRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;

namespace Iptv.Web.Pages.Identity;

public partial class RegisterPage : ComponentBase
{
    private bool PageIsBusy { get; set; }
    private bool RegisterIsBusy { get; set; }
    private RegisterRequest Request { get; set; } = new();
    
    private bool Redirect { get; set; }
    private string? ProductId { get; set; }
    private string? Quantity { get; set; }
    
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public IIdentityHandler IdentityHandler { get; set; } = null!;
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    
    protected override async Task OnInitializedAsync()
    {
        PageIsBusy = true;
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                NavigationManager.NavigateTo("/");
            }
            
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var query = QueryHelpers.ParseQuery(uri.Query);
        
            if (query.TryGetValue("redirect", out var redirect) &&
                query.TryGetValue("product", out var product) &&
                query.TryGetValue("quantity", out var quantity))
            {
                Redirect = bool.Parse(redirect!);
                ProductId = product.FirstOrDefault();
                Quantity = quantity.FirstOrDefault();
            }
        }
        catch
        {
            Snackbar.Add($"Erro ao carregar a página", Severity.Error);
        }
        finally
        {
            PageIsBusy = false;
        }
    }

    private async Task Register()
    {
        RegisterIsBusy = true;
        try
        {
            var result = await IdentityHandler.RegisterAsync(Request);

            if (result.IsSuccess)
            {
                if (Redirect && !string.IsNullOrEmpty(ProductId) && !string.IsNullOrEmpty(Quantity))
                {
                    Snackbar.Add(result.Message ?? "Cadastro concluido com sucesso! Faça login agora", Severity.Success);
                    NavigationManager.NavigateTo($"/entrar?redirect={Redirect.ToString()}&product={ProductId}&quantity={Quantity}");
                }
                else
                {
                    Snackbar.Add(result.Message ?? "Cadastro concluido com sucesso! Faça login agora", Severity.Success);
                    NavigationManager.NavigateTo("/entrar");
                }
            }
            else
            {
                Snackbar.Add(result.Message ?? "Erro ao fazer o cadastro", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            RegisterIsBusy = false;
        }
    }
}