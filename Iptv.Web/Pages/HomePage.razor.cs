using System.Security.Claims;
using Iptv.Web.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Iptv.Web.Pages;

public partial class HomePage : ComponentBase
{
    private bool PageIsBusy { get; set; } = false;

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


}