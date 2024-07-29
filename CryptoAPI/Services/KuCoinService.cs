using CryptoAPI.Models.RequestModels;
using CryptoAPI.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace CryptoAPI.Services;

public class KuCoinService : IExchangeService
{
    private readonly HttpClient _httpClient;

    public KuCoinService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetPrice(string baseCurrency, string quoteCurrency)
    {
        var response = await _httpClient.GetAsync($"api/v1/market/orderbook/level1?symbol={baseCurrency}-{quoteCurrency}");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var json = JObject.Parse(content);

        return json["data"]["price"].Value<decimal>();
    }
}