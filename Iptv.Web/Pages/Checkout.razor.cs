using Iptv.Core.Models;
using Microsoft.AspNetCore.Components;

namespace Iptv.Web.Pages;

public partial class Checkout : ComponentBase
{
    public Address Address { get; set; } = new();

    public TvBox TvBox { get; set; } = new();
    
    [Parameter]
    [SupplyParameterFromQuery(Name = "product")]
    public int ProductId { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "quantity")]
    public int Quantity { get; set; }
}