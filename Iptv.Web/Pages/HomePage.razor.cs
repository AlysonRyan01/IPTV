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
    bool _contato = false;
    public ElementReference PrimeiraSection;
    public ElementReference SegundaSection;
    public ElementReference TerceiraSection;
    public ElementReference QuartaSection;
    public ElementReference QuintaSection;

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
}