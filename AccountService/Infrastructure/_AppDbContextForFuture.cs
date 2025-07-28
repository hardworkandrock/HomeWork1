using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Emit; 
using Microsoft.EntityFrameworkCore;
using Common.Src;

namespace AccountService.Data
{
    /// <summary>
    /// Для работы с бд
    /// </summary>
    public class _AppDbContextForFuture : DbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public _AppDbContextForFuture(DbContextOptions<_AppDbContextForFuture> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(w => w.Id);
                entity.HasOne(w => w.Owner)
                      .WithMany(o => o.Wallets)
                      .HasForeignKey(w => w.OwnerId);
                entity.ToTable("Wallets");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id); 
                entity.HasOne(t => t.Account)
                      .WithMany(w => w.Transactions)
                      .HasForeignKey(t => t.AccountId);
                entity.ToTable("Transactions");
            });

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.ToTable("Owners");
            });

            modelBuilder.HasPostgresEnum<TypeWallet>();
            modelBuilder.HasPostgresEnum<TransactionType>();
        }
    }
}
