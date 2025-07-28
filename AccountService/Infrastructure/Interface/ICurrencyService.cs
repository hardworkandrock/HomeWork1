namespace AccountService.Infrastructure.Interface;

public interface ICurrencyService
{
    Task<bool> IsSupportedAsync(string currency, CancellationToken ct);
}