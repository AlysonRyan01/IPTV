using Iptv.Core.Handlers;
using Iptv.Core.Requests.IdentityRequests;
using Iptv.Web.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;

namespace Iptv.Web.Pages.Identity;

public partial class LoginPage : ComponentBase
{
    private bool PageIsBusy { get; set; }
    private bool LoginIsBusy { get; set; }
    private LoginRequest Request { get; set; } = new();
    
    private string? ProductId { get; set; }
    private string? Quantity { get; set; }
    private bool Redirect { get; set; }
    
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public IIdentityHandler IdentityHandler { get; set; } = null!;
    [Inject] public AuthStateProvider AuthStateProvider { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        PageIsBusy = true;
        try
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
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

    private async Task Login()
    {
        LoginIsBusy = true;
        try
        {
            var result = await IdentityHandler.LoginAsync(Request);
            
            if (result.IsSuccess)
            {
                await AuthStateProvider.NotifyUserAuthentication();
                if (Redirect && !string.IsNullOrEmpty(ProductId) && !string.IsNullOrEmpty(Quantity))
                {
                    Snackbar.Add(result.Message ?? "Login Realizado com sucesso!", Severity.Success);
                    NavigationManager.NavigateTo($"/checkout?redirect={Redirect.ToString()}&product={ProductId}&quantity={Quantity}");
                }
                else
                {
                    Snackbar.Add(result.Message ?? "Login Realizado com sucesso!", Severity.Success);
                    NavigationManager.NavigateTo("/");
                }
            }
            else
            {
                Snackbar.Add(result.Message ?? "Falha ao realizar o login", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            LoginIsBusy = false;
        }
    }
}