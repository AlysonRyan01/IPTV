using Iptv.Api.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseSecurity();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();


app.MapGet("/", () => "api rodando");

app.Run();

