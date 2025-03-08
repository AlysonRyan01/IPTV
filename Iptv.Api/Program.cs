using Iptv.Api.Common;
using Iptv.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddCorsConfiguration();
builder.AddServices();
builder.AddSwaggerGen();
builder.AddIdentity();
builder.AddDbConfiguration();
builder.AddConfigurationApiUrl();
builder.AddControllers();

var app = builder.Build();

app.UseSecurity();

app.UseHttpsRedirection();

app.MapControllers();

if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();


app.MapGet("/", (TokenService tokenService) =>
    tokenService.Generate(null)
);

app.Run();

