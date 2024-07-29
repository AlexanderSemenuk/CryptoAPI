using CryptoAPI.Endpoints;
using CryptoAPI.Services;
using CryptoAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpClient("Binance", c =>
{
    c.BaseAddress = new Uri("https://api.binance.com/");
});

builder.Services.AddTransient<IExchangeService>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("Binance");
    return new BinanceService(httpClient);
});

builder.Services.AddHttpClient("KuCoin", c =>
{
    c.BaseAddress = new Uri("https://api.kucoin.com/");
});

builder.Services.AddTransient<IExchangeService>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("KuCoin");
    return new KuCoinService(httpClient);
});


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

