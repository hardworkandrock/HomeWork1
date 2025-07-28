namespace AccountService.Infrastructure.Interface
{
    public interface IClientVerificationService
    {
        Task<bool> ExistsAsync(Guid ownerId, CancellationToken ct);
    }
}
