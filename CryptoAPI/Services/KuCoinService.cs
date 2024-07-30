using CryptoAPI.Services.Interfaces;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CryptoAPI.Services;

public class KuCoinService : IExchangeService
{
    private readonly RestClient _restClient;

    public KuCoinService()
    {
        _restClient = new RestClient("https://api.kucoin.com/");
    }

    public async Task<decimal> GetPrice(string baseCurrency, string quoteCurrency)
    {
        try
        {

            var initialPair = $"{baseCurrency}-{quoteCurrency}";
            var initialPrice = await GetPriceFromApi(initialPair);
            return initialPrice;
        }
        catch
        {

            var reversedPair = $"{quoteCurrency}-{baseCurrency}";
            var reversedPrice = await GetPriceFromApi(reversedPair);

            var invertedPrice = 1 / reversedPrice;
            return invertedPrice;
        }
    }

    private async Task<decimal> GetPriceFromApi(string pair)
    {
        var request = new RestRequest($"api/v1/market/orderbook/level1?symbol={pair}", Method.Get);
        var response = await _restClient.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            throw new Exception($"API request failed for pair {pair}");
        }

        var json = JObject.Parse(response.Content);
        var data = json["data"];

        if (data == null)
        {
            throw new Exception("Invalid JSON response: 'data' field is missing.");
        }

        var price = data.Value<decimal?>("price");

        if (price == null)
        {
            throw new Exception("Invalid JSON response: 'price' field is missing or not a decimal.");
        }

        return price.Value;
    }
}