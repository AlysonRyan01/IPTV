using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
namespace Iptv.Web.Authentication;

public class AuthStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        
        return Task.FromResult(new AuthenticationState(user));
    }
}