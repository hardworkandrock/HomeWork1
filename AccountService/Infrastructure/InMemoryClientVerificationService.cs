using AccountService.Infrastructure.Interface;

namespace AccountService.Infrastructure
{
    public class InMemoryClientVerificationService : IClientVerificationService
    {
        private readonly HashSet<Guid> _validClientIds = new()
    {
        Guid.Parse("11111111-1111-1111-1111-111111111111"), // Тестовый клиент
        Guid.Parse("22222222-2222-2222-2222-222222222222")
    };

        public Task<bool> ExistsAsync(Guid ownerId, CancellationToken ct)
        {
            return Task.FromResult(_validClientIds.Contains(ownerId));
        }
    }
}
