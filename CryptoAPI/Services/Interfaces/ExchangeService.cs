using CryptoAPI.Models.RequestModels;

namespace CryptoAPI.Services.Interfaces;

public interface IExchangeService
{
    Task<decimal> GetPrice(string baseCurrency, string quoteCurrency);
}