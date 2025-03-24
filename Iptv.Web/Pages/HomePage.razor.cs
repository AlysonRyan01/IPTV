using System.Security.Claims;
using Iptv.Core.Handlers;
using Iptv.Core.Models;
using Iptv.Web.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;

namespace Iptv.Web.Pages;

public partial class HomePage : ComponentBase
{
    private bool _open;
    private bool UserLoggedIn { get; set; }
    public UserInfo UserInfo { get; set; } = new ();
    private string Username { get; set; } = string.Empty;
    private readonly Anchor _anchor = Anchor.Left;
    private bool PageIsBusy { get; set; }
    private int Quantity { get; set; } = 1;
    bool _pergunta1;
    bool _pergunta2;
    bool _pergunta3;
    bool _pergunta4;
    bool _pergunta5;
    bool _contato;
    public ElementReference PrimeiraSection;
    public ElementReference SegundaSection;
    public ElementReference TerceiraSection;
    public ElementReference QuartaSection;
    public ElementReference QuintaSection;

    [Inject] private AuthStateProvider AuthStateProvider { get; set; } = null!;
    [Inject] private IIdentityHandler IdentityHandler { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        PageIsBusy = true;
        try
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                UserLoggedIn = true;
                Username = user.Identity.Name ?? string.Empty;
                
                var result = await IdentityHandler.UserInfo(null);
                if (result.IsSuccess)
                    UserInfo = result.Data ?? new UserInfo();
                else
                    Snackbar.Add($"Erro: {result.Message}", Severity.Error);
                
            }
        }
        catch
        {
            Snackbar.Add("Ocorreu um erro ao carregar os dados.", Severity.Error);
        }
        finally
        {
            PageIsBusy = false;
        }
    }

    private void OpenDrawer()
    {
        _open = true;
    }
    
    private async Task ScrollToPrimeiraSection()
    {
        _open = false;
        await JsRuntime.InvokeVoidAsync("scrollToSection", PrimeiraSection);
    }
    
    private async Task ScrollToSegundaSection()
    {
        _open = false;
        await JsRuntime.InvokeVoidAsync("scrollToSection", SegundaSection);
    }
    
    private async Task ScrollToTerceiraSection()
    {
        _open = false;
        await JsRuntime.InvokeVoidAsync("scrollToSection", TerceiraSection);
    }
    
    private async Task ScrollToQuartaSection()
    {
        _open = false;
        await JsRuntime.InvokeVoidAsync("scrollToSection", QuartaSection);
    }
    
    private async Task ScrollToQuintaSection()
    {
        _open = false;
        await JsRuntime.InvokeVoidAsync("scrollToSection", QuintaSection);
    }
    
    private void AbrirPergunta1() {
        _pergunta1 = !_pergunta1;
    }
    
    private void AbrirPergunta2() {
        _pergunta2 = !_pergunta2;
    }
    
    private void AbrirPergunta3() {
        _pergunta3 = !_pergunta3;
    }
    
    private void AbrirPergunta4() {
        _pergunta4 = !_pergunta4;
    }
    
    private void AbrirPergunta5() {
        _pergunta5 = !_pergunta5;
    }
    
    private void AbrirContato() {
        _contato = !_contato;
    }
}