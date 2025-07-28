using Common.Src;

namespace AccountService.Infrastructure.Interface
{
    public interface IOwnerService
    {
        Task<Owner?> GetByIdAsync(Guid id, CancellationToken ct);
    }
}
