using CryptoAPI.Models.RequestModels;
using CryptoAPI.Models.ResponseModels;
using CryptoAPI.Services.Interfaces;

namespace CryptoAPI.Endpoints;

public static class GetRatesEndpoint
{
    
    public static void MapGetRatesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/exchange/getRates", async (GetRatesRequest request, IEnumerable<IExchangeService> exchangeServices) =>
        {
            var rates = await GetRates(request, exchangeServices);
            return Results.Ok(rates);
        });
    }
    private static async Task<IEnumerable<GetRatesResponse>> GetRates(GetRatesRequest request,
        IEnumerable<IExchangeService> exchangeServices)
    {
        var tasks = exchangeServices.Select(async service =>
        {
            var rate = await service.GetPrice(request.BaseCurrency, request.QuoteCurrency);

            return new GetRatesResponse
            {
                ExchangeName = GetExchange(service.GetType().Name),
                Rate = rate
            };
        }).ToList();
        
        return await Task.WhenAll(tasks);
    }

    private static string GetExchange(string fullTypeName)
    {
        return fullTypeName.Replace("Service", "");
    }
}