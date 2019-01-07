using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace split_api.Models
{
    public partial class transactionsContext : DbContext
    {
        public transactionsContext()
        {
        }

        public transactionsContext(DbContextOptions<transactionsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<TransactionParties> TransactionParties { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryType).HasColumnName("categoryType");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<TransactionParties>(entity =>
            {
                entity.ToTable("Transaction_Parties");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DefaultCategory).HasColumnName("defaultCategory");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.HasOne(d => d.DefaultCategoryNavigation)
                    .WithMany(p => p.TransactionParties)
                    .HasForeignKey(d => d.DefaultCategory)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Transaction_Party_defaultCategory_fkey");
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountIn).HasColumnName("accountIn");

                entity.Property(e => e.AccountOut).HasColumnName("accountOut");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("money");

                entity.Property(e => e.Category).HasColumnName("category");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.IsShared).HasColumnName("isShared");

                entity.Property(e => e.TransactionParty).HasColumnName("transactionParty");

                entity.HasOne(d => d.AccountInNavigation)
                    .WithMany(p => p.TransactionsAccountInNavigation)
                    .HasForeignKey(d => d.AccountIn)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Transactions_accountOut_fkey");

                entity.HasOne(d => d.AccountOutNavigation)
                    .WithMany(p => p.TransactionsAccountOutNavigation)
                    .HasForeignKey(d => d.AccountOut)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Transactions_account_fkey");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Transactions_category_fkey");

                entity.HasOne(d => d.TransactionPartyNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TransactionParty)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Transactions_transactionParty_fkey");
            });
        }
    }
}
