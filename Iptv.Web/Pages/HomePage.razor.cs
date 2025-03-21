using System.Security.Claims;
using Iptv.Web.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;

namespace Iptv.Web.Pages;

public partial class HomePage : ComponentBase
{
    private bool _open;
    private readonly Anchor _anchor = Anchor.Left;
    private bool PageIsBusy { get; set; }
    private int Quantidade { get; set; } = 1;
    bool _pergunta1 = false;
    bool _pergunta2 = false;
    bool _pergunta3 = false;
    bool _pergunta4 = false;
    bool _pergunta5 = false;

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

    [Inject] public AuthStateProvider AuthStateProvider { get; set; } = null!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        PageIsBusy = true;
        try
        {
            
        }
        catch
        {

        }
        finally
        {
            PageIsBusy = false;
        }
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsRuntime.InvokeVoidAsync("initializePlyr");
        }
    }

    private void OpenDrawer()
    {
        _open = true;
    }
}