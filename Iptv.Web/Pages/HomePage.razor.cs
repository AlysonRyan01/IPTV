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

    [Inject] public AuthStateProvider AuthStateProvider { get; set; } = null!;

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
}