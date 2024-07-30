using CryptoAPI.Services;
using CryptoAPI.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace CryptoAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddExchangeService(this IServiceCollection services, string exchangeName)
        {
            services.AddTransient<IExchangeService>(sp =>
            {
                return exchangeName switch
                {
                    "Binance" => new BinanceService(),
                    "KuCoin" => new KuCoinService(),
                    _ => throw new ArgumentException("Unknown exchange")
                };
            });

            return services;
        }
    }
}
