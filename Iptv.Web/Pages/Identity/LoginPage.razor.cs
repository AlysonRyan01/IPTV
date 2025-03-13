using Iptv.Core.Handlers;
using Iptv.Core.Requests.IdentityRequests;
using Iptv.Web.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Iptv.Web.Pages.Identity;

public partial class LoginPage : ComponentBase
{
    private bool PageIsBusy { get; set; }
    private bool LoginIsBusy { get; set; }
    private LoginRequest Request { get; set; } = new();
    
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
                Snackbar.Add(result.Message ?? "Login Realizado com sucesso!", Severity.Success);
                NavigationManager.NavigateTo("/");
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