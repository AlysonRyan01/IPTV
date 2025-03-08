using Iptv.Api.Common;

var builder = WebApplication.CreateBuilder(args);

builder.AddCorsConfiguration();
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


app.MapGet("/", () => "api rodando");

app.Run();

