using AccountService.Infrastructure.Interface;
using Common.Src;

namespace AccountService.Infrastructure;

public class InMemoryOwnerService : IOwnerService
{
    private readonly List<Owner> _owners = new()
    {
        new Owner
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), FirstName = "Иван", LastName = "Иванов",
            FatherName = "Иванович"
        },
        new Owner
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), FirstName = "Мария", LastName = "Петрова",
            FatherName = "Сергеевна"
        }
    };

    public Task<Owner?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return Task.FromResult(_owners.FirstOrDefault(o => o.Id == id));
    }
}