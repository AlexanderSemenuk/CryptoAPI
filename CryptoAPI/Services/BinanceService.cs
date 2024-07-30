using CryptoAPI.Services.Interfaces;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CryptoAPI.Services;

public class BinanceService : IExchangeService
{
    private readonly RestClient _restClient;

    public BinanceService()
    {
        _restClient = new RestClient("https://api.binance.com/");
    }

    public async Task<decimal> GetPrice(string baseCurrency, string quoteCurrency)
    {
        try
        {
            var initialPair = $"{baseCurrency}{quoteCurrency}";
            var initialPrice = await GetPriceFromApi(initialPair);
            return initialPrice;
        }
        catch
        {
            var reversedPair = $"{quoteCurrency}{baseCurrency}";
            var reversedPrice = await GetPriceFromApi(reversedPair);
            var invertedPrice = 1 / reversedPrice;
            return invertedPrice;
        }
    }

    private async Task<decimal> GetPriceFromApi(string pair)
    {
        var request = new RestRequest($"api/v3/ticker/price?symbol={pair}", Method.Get);
        var response = await _restClient.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            throw new Exception($"API request failed for pair {pair}");
        }

        var json = JObject.Parse(response.Content);
        return json.Value<decimal>("price");
    }
}