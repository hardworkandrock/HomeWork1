namespace Common.Src
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid? CounterPartyAccountId { get; set; }
        /// <summary>
        /// Сумма перевода
        /// </summary>
        public decimal TransferAmount { get; set; }
        /// <summary>
        /// Валюта ISO 4217
        /// </summary>
        public string Currency {  get; set; } = string.Empty;
        /// <summary>
        /// Тип дебит/кредит
        /// </summary>
        public TransactionType TransactionType { get; set; }
        /// <summary>
        /// Описание. Примеры: 
        ///     Открыт вклад "Надежный-6" под 3%
        ///     Пополнение наличными
        ///     Перевод
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Дата создания операции
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// AccountId
        /// </summary>
        public virtual Wallet Account { get; set; } = null!;
    }
    public enum TransactionType
    {
        Credit = 0,
        Debit = 1
    }
}
