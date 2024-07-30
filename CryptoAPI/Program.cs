using CryptoAPI.Endpoints;
using CryptoAPI.Extensions;
using CryptoAPI.Services;
using CryptoAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddExchangeService("Binance");
builder.Services.AddExchangeService("KuCoin");


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEstimateEndpoint();
app.MapGetRatesEndpoint();

app.Run();

