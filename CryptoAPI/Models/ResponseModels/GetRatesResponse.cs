namespace CryptoAPI.Models.ResponseModels;

public class GetRatesResponse
{
    public string ExchangeName { get; set; }
    
    public decimal Rate { get; set; }
}