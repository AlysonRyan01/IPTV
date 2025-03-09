using Iptv.Api;
using Iptv.Api.Common;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfigurationApiUrl();
builder.AddAuthorizationConfiguration();
builder.AddCorsConfiguration();
builder.AddJwtConfiguration();
builder.AddServices();
builder.AddSwaggerGen();
builder.AddIdentity();
builder.AddDbConfiguration();
builder.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(ApiConfiguration.CorsPolicyName);

if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.MapControllers();

app.MapGet("/", () => "Api rodando");

app.Run();

