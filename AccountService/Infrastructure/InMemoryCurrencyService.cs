using AccountService.Infrastructure.Interface;

namespace AccountService.Infrastructure
{
    public class InMemoryCurrencyService : ICurrencyService
    {
        private readonly HashSet<string> _supportedCurrencies = new(StringComparer.OrdinalIgnoreCase)
    {
        "RUB", "USD", "EUR", "KZT", "CNY"
    };

        public Task<bool> IsSupportedAsync(string currency, CancellationToken ct)
        {
            return Task.FromResult(_supportedCurrencies.Contains(currency));
        }
    }
}
