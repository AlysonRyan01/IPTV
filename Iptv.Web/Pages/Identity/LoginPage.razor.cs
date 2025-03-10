using Iptv.Core.Handlers;
using Iptv.Core.Requests.IdentityRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Iptv.Web.Pages.Identity;

public partial class LoginCode : ComponentBase
{
    public bool IsBusy { get; set; } = false;
    public LoginRequest Request { get; set; } = new();
    
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public IIdentityHandler IdentityHandler { get; set; } = null!;
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        
        if (user.Identity?.IsAuthenticated == true)
        {
            NavigationManager.NavigateTo("/");
        }
    }

    public async Task Login()
    {
        IsBusy = true;
        try
        {
            var result = await IdentityHandler.LoginAsync(Request);

            if (result.IsSuccess)
            {
                Snackbar.Add(result?.Message ?? "Login Realizado com sucesso!", Severity.Success);
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
            IsBusy = false;
        }
    }
}