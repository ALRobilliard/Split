using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SplitApi.Models
{
    public partial class SplitContext : DbContext
    {
        public SplitContext()
        {
        }

        public SplitContext(DbContextOptions<SplitContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<TransactionParty> TransactionParties { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryType).HasColumnName("categoryType");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<TransactionParty>(entity =>
            {
                entity.ToTable("Transaction_Party");

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

            modelBuilder.Entity<Transaction>(entity =>
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
                    .WithMany(p => p.TransactionAccountInNavigation)
                    .HasForeignKey(d => d.AccountIn)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Transaction_accountIn_fkey");

                entity.HasOne(d => d.AccountOutNavigation)
                    .WithMany(p => p.TransactionAccountOutNavigation)
                    .HasForeignKey(d => d.AccountOut)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Transaction_accountOut_fkey");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Transaction_category_fkey");

                entity.HasOne(d => d.TransactionPartyNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TransactionParty)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Transaction_transactionParty_fkey");
            });
        }
    }
}
