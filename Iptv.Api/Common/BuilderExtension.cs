using System.Text.Json;
using Iptv.Api.Data;
using Iptv.Api.Models;
using Iptv.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Iptv.Api.Common;

public static class BuilderExtension
{
    public static void AddDbConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<IptvDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    }
    
    public static void AddIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddDataProtection();
        
        builder.Services.AddIdentityCore<User>()
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<IptvDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
    }
    
    public static void AddConfigurationApiUrl(this WebApplicationBuilder builder)
    {
        Configuration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? String.Empty;
        Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? String.Empty;
        Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? String.Empty;
    }
    
    public static void AddControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true; 
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; 
            });
    }
    
    public static void AddCorsConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options.AddPolicy(
            ApiConfiguration.CorsPolicyName,
            policy => policy
                .WithOrigins(
                [
                    Configuration.BackendUrl,
                    Configuration.FrontendUrl
                ])
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
        ));
    }
}