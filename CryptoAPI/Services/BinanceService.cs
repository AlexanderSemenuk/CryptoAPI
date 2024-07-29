using System.Text.Json.Nodes;
using CryptoAPI.Models.RequestModels;
using CryptoAPI.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace CryptoAPI.Services;

public class BinanceService : IExchangeService
{
    private readonly HttpClient _httpClient;
    public BinanceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetPrice(GetRatesRequest request)
    {
        var response =
            await _httpClient.GetAsync($"api/v3/ticker/price?symbol={request.BaseCurrency}{request.QuoteCurrency}");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var json = JObject.Parse(content);
        
        return json.Value<decimal>("price");
    }
}