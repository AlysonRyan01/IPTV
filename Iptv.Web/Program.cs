using Blazored.LocalStorage;
using Iptv.Core;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Iptv.Web;
using Iptv.Web.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();


Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? String.Empty;
WebConfiguration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? String.Empty;

builder.Services.AddHttpClient(WebConfiguration.HttpClientName,
    client =>
    {
        client.BaseAddress = new Uri(WebConfiguration.BackendUrl);
    }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

await builder.Build().RunAsync();
