using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Common.Src
{
    public class Wallet
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Client хранит ФИО
        /// </summary>
        public Guid OwnerId {  get; set; }
        public TypeWallet TypeWallet {  get; set; }
        public decimal? InterestRate { get; set; } 
        public string Currency { get; set; }
        public decimal Balance {  get; set; } 
        public DateTime DateOpen { get; set; }
        public DateTime? DateClosed { get; set; }

        public virtual Owner Owner { get; set; } = null!;
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    } 

    public enum TypeWallet
    {
        Checking = 0,
        Deposit = 1,
        Credit = 2
    }
} 
  