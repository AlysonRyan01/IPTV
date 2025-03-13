using System.Security.Claims;
using Iptv.Core.Handlers;
using Iptv.Core.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Iptv.Web.Authentication;

public class AuthStateProvider(IIdentityHandler identityHandler) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userInfoResponse = await identityHandler.UserInfo(string.Empty);

            if (userInfoResponse == null || !userInfoResponse.IsSuccess || userInfoResponse.Data == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = GetClaims(userInfoResponse.Data);
            
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
            }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public List<Claim> GetClaims(UserInfo userInfo)
    {
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userInfo.UserId.ToString()),
            new Claim(ClaimTypes.Name, userInfo.FullName),
            new Claim(ClaimTypes.Email, userInfo.Email),
            new Claim("PhoneNumber", userInfo.Phone),
            new Claim("IsAdmin", userInfo.IsAdmin.ToString())
        };

        return claims;
    }

    public async Task NotifyUserAuthentication()
    {
        try
        {
            var userInfoResponse = await identityHandler.UserInfo(string.Empty);

            if (userInfoResponse == null || !userInfoResponse.IsSuccess || userInfoResponse.Data == null)
            {
                NotifyUserLogout();
                return;
            }

            var claims = GetClaims(userInfoResponse.Data);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    catch
    {
        NotifyUserLogout();
    }
    }

    public void NotifyUserLogout()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
    }
}