namespace Iptv.Web.Services;

using Microsoft.JSInterop;

public interface ICookieService
{
    Task<string> GetCookieAsync(string cookieName);
    Task RemoveCookieAsync(string cookieName);
}

public class CookieService : ICookieService
{
    private readonly IJSRuntime _jsRuntime;

    public CookieService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetCookieAsync(string cookieName)
    {
        return await _jsRuntime.InvokeAsync<string>("getCookie", cookieName);
    }

    public async Task RemoveCookieAsync(string cookieName)
    {
        await _jsRuntime.InvokeVoidAsync("removeCookie", cookieName);
    }
}