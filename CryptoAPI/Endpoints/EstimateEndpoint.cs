using CryptoAPI.Models.RequestModels;
using CryptoAPI.Models.ResponseModels;
using CryptoAPI.Services.Interfaces;

namespace CryptoAPI.Endpoints;

public static class EstimateEndpoint
{
    public static void MapEstimateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/exchange/estimate", async (EstimateRequest request, IEnumerable<IExchangeService> exchangeServices) =>
        {
            var bestExchange = await GetBestExchange(request, exchangeServices);
            var outputAmount = request.InputAmount * bestExchange.Price;
            return Results.Ok(new EstimateResponse
            {
                ExchangeName = GetExchange(bestExchange.ExchangeName),
                OutputAmount = outputAmount
            });
        });
    }

    private static async Task<dynamic> GetBestExchange(EstimateRequest request,
        IEnumerable<IExchangeService> exchangeServices)
    {
        var tasks = exchangeServices.Select(async service =>
        {
            var price = await service.GetPrice(request.InputCurrency, request.OutputCurrency);

            return new { ExchangeName = GetExchange(service.GetType().Name), Price = price };
        }).ToList();

        var results = await Task.WhenAll(tasks);

        return results.OrderByDescending(r => r.Price).First();
    }

    private static string GetExchange(string fullTypeName)
    {
        return fullTypeName.Replace("Service", "");
    }
}