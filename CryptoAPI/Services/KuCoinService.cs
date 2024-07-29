using CryptoAPI.Models.RequestModels;
using Newtonsoft.Json.Linq;

namespace CryptoAPI.Services;

public class KuCoinService
{
    private readonly HttpClient _httpClient;

    public KuCoinService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetPrice(GetRatesRequest request)
    {
        var response = await _httpClient.GetAsync($"api/v1/market/orderbook/level1?symbol={request.BaseCurrency}-{request.QuoteCurrency}");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var json = JObject.Parse(content);

        return json["data"]["price"].Value<decimal>();
    }
}