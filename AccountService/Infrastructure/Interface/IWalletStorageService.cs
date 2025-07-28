using Common.Src;

namespace AccountService.Infrastructure.Interface;

/// <summary>
///     Сервис для хранения и управления банковскими счетами.
///     Абстрагирует доступ к данным (БД, in-memory, внешний API и т.д.)
/// </summary>
public interface IWalletStorageService
{
    /// <summary>
    ///     Создаёт новый счёт
    /// </summary>
    /// <param name="wallet">Счёт для создания</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Созданный счёт</returns>
    Task<Wallet> CreateAsync(Wallet wallet, CancellationToken ct);

    /// <summary>
    ///     Возвращает счёт по ID
    /// </summary>
    /// <param name="id">Идентификатор счёта</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Счёт или null, если не найден</returns>
    Task<Wallet?> GetByIdAsync(Guid id, CancellationToken ct);

    /// <summary>
    ///     Возвращает список счетов
    /// </summary>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Список счетов</returns>
    Task<IEnumerable<Wallet>> GetAllAsync(CancellationToken ct);

    /// <summary>
    ///     Возвращает все счета владельца
    /// </summary>
    /// <param name="ownerId">Идентификатор владельца</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Список счетов</returns>
    Task<IEnumerable<Wallet>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct);

    /// <summary>
    ///     Обновляет счёт
    /// </summary>
    /// <param name="wallet">Обновлённый счёт</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns></returns>
    Task UpdateAsync(Wallet wallet, CancellationToken ct);
     
    /// <summary>
    ///     Добавляет транзакцию
    /// </summary>
    /// <param name="transaction">Транзакция</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns></returns>
    Task AddTransactionAsync(Transaction transaction, CancellationToken ct);

    /// <summary>
    ///     Возвращает транзакции счёта
    /// </summary>
    /// <param name="accountId">Идентификатор счёта</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Список транзакций</returns>
    Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(Guid accountId, CancellationToken ct);
}