using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Iptv.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Iptv.Api.Services;

public class TokenService(UserManager<User> userManager)
{
    public async Task<string> Generate(User user)
    {
        var handler = new JwtSecurityTokenHandler();
        
        var key = Encoding.ASCII.GetBytes(ApiConfiguration.JwtKey);

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = await GenerateClaims(user),
            SigningCredentials = credentials,   
            Expires = DateTime.UtcNow.AddHours(8),
            
        };

        var token = handler.CreateToken(tokenDescriptor);
        
        return handler.WriteToken(token);
        
    }

    private async Task<ClaimsIdentity> GenerateClaims(User user)
    {
        var ci = new ClaimsIdentity();
        
        ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        ci.AddClaim(new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"));
        ci.AddClaim(new Claim(ClaimTypes.Email, user.Email ?? string.Empty));
        ci.AddClaim(new Claim("IsAdmin", user.IsAdmin.ToString()));
        
        var roles = await userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            ci.AddClaim(new Claim(ClaimTypes.Role, role));
        }
        
        return ci;
    }

    public void SetTokensInsideCookie(string token, HttpContext context)
    {
        context.Response.Cookies.Append("accessToken", token, new CookieOptions
        {
            Expires = DateTime.UtcNow.AddHours(2),
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Strict
        });
    }
}

