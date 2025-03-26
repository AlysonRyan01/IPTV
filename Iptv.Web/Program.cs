using Iptv.Core;
using Iptv.Core.Handlers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Iptv.Web;
using Iptv.Web.Authentication;
using Iptv.Web.Handlers;
using Iptv.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor.Services;
using System.Net.Http.Headers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<CookieHandler>();

Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? String.Empty;
WebConfiguration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? String.Empty;

builder.Services.AddScoped<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddTransient<IdentityServices>();
builder.Services.AddTransient<IIdentityHandler, IdentityHandler>();
builder.Services.AddTransient<ITvboxHandler, TvboxHandler>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("IsAdmin", "True"));
});
builder.Services.AddMudServices();


builder.Services.AddHttpClient("identity", client =>
{
    client.BaseAddress = new Uri($"{WebConfiguration.BackendUrl}/v1/identity/");
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<CookieHandler>();

builder.Services.AddHttpClient("tvbox", client =>
    {
        client.BaseAddress = new Uri($"{WebConfiguration.BackendUrl}/v1/tvbox/");
        client.Timeout = TimeSpan.FromSeconds(60);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    })
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<AuthStateProvider>());

await builder.Build().RunAsync();
