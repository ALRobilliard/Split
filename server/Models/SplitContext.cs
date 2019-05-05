using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Split.Models
{
  public partial class SplitContext : DbContext
  {
    private readonly string _connectionString;

    public SplitContext()
    {
    }

    public SplitContext(DbContextOptions<SplitContext> options)
        : base(options)
    {
    }

    public SplitContext(string connectionString)
    {
      this._connectionString = connectionString;
    }

    public virtual DbSet<Account> Account { get; set; }
    public virtual DbSet<Category> Category { get; set; }
    public virtual DbSet<SplitPayment> SplitPayment { get; set; }
    public virtual DbSet<Transaction> Transaction { get; set; }
    public virtual DbSet<TransactionParty> TransactionParty { get; set; }
    public virtual DbSet<User> User { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseNpgsql(this._connectionString);
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ForNpgsqlHasEnum(null, "categorytype", new[] { "income", "expense" })
          .HasPostgresExtension("uuid-ossp");

      modelBuilder.Entity<Account>(entity =>
      {
        entity.Property(e => e.AccountId)
                  .HasColumnName("accountId")
                  .HasDefaultValueSql("uuid_generate_v4()");

        entity.Property(e => e.AccountName)
                  .IsRequired()
                  .HasColumnName("accountName")
                  .HasColumnType("character varying(20)");

        entity.Property(e => e.AccountType).HasColumnName("accountType");

        entity.Property(e => e.Balance)
                  .HasColumnName("balance")
                  .HasColumnType("money");

        entity.Property(e => e.CreatedOn)
                  .HasColumnName("createdOn")
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.Limit)
                  .HasColumnName("limit")
                  .HasColumnType("money");

        entity.Property(e => e.ModifiedOn)
                  .HasColumnName("modifiedOn")
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.UserId).HasColumnName("userId");

        entity.HasOne(d => d.User)
                  .WithMany(p => p.Account)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("Account_userId_fkey");
      });

      modelBuilder.Entity<Category>(entity =>
      {
        entity.Property(e => e.CategoryId)
                  .HasColumnName("categoryId")
                  .HasDefaultValueSql("uuid_generate_v4()");

        entity.Property(e => e.CategoryName)
                  .IsRequired()
                  .HasColumnName("categoryName")
                  .HasColumnType("character varying(25)");

        entity.Property(e => e.CategoryType).HasColumnName("categoryType");

        entity.Property(e => e.CreatedOn)
                  .HasColumnName("createdOn")
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.ModifiedOn)
                  .HasColumnName("modifiedOn")
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.UserId).HasColumnName("userId");

        entity.HasOne(d => d.User)
                  .WithMany(p => p.Category)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("Category_userId_fkey");
      });

      modelBuilder.Entity<SplitPayment>(entity =>
      {
        entity.Property(e => e.SplitPaymentId)
                  .HasColumnName("splitPaymentId")
                  .HasDefaultValueSql("uuid_generate_v4()");

        entity.Property(e => e.Amount)
                  .HasColumnName("amount")
                  .HasColumnType("money");

        entity.Property(e => e.CreatedOn)
                  .HasColumnName("createdOn")
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.ModifiedOn)
                  .HasColumnName("modifiedOn")
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.PayeeId).HasColumnName("payeeId");

        entity.Property(e => e.TransactionId).HasColumnName("transactionId");

        entity.HasOne(d => d.Payee)
                  .WithMany(p => p.SplitPayment)
                  .HasForeignKey(d => d.PayeeId)
                  .HasConstraintName("SplitPayment_payeeId_fkey");

        entity.HasOne(d => d.Transaction)
                  .WithMany(p => p.SplitPayment)
                  .HasForeignKey(d => d.TransactionId)
                  .HasConstraintName("SplitPayment_transactionId_fkey");
      });

      modelBuilder.Entity<Transaction>(entity =>
      {
        entity.Property(e => e.TransactionId)
                  .HasColumnName("transactionId")
                  .HasDefaultValueSql("uuid_generate_v4()");

        entity.Property(e => e.AccountInId).HasColumnName("accountInId");

        entity.Property(e => e.AccountOutId).HasColumnName("accountOutId");

        entity.Property(e => e.Amount)
                  .HasColumnName("amount")
                  .HasColumnType("money");

        entity.Property(e => e.CategoryId).HasColumnName("categoryId");

        entity.Property(e => e.CreatedOn)
                  .HasColumnName("createdOn")
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.IsShared).HasColumnName("isShared");

        entity.Property(e => e.ModifiedOn)
                  .HasColumnName("modifiedOn")
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.TransactionDate)
                  .HasColumnName("transactionDate")
                  .HasColumnType("date")
                  .HasDefaultValueSql("CURRENT_DATE");

        entity.Property(e => e.TransactionPartyId).HasColumnName("transactionPartyId");

        entity.Property(e => e.UserId).HasColumnName("userId");

        entity.HasOne(d => d.AccountIn)
                  .WithMany(p => p.TransactionAccountIn)
                  .HasForeignKey(d => d.AccountInId)
                  .HasConstraintName("Transaction_accountInId_fkey");

        entity.HasOne(d => d.AccountOut)
                  .WithMany(p => p.TransactionAccountOut)
                  .HasForeignKey(d => d.AccountOutId)
                  .HasConstraintName("Transaction_accountOutId_fkey");

        entity.HasOne(d => d.Category)
                  .WithMany(p => p.Transaction)
                  .HasForeignKey(d => d.CategoryId)
                  .OnDelete(DeleteBehavior.SetNull)
                  .HasConstraintName("Transaction_categoryId_fkey");

        entity.HasOne(d => d.TransactionParty)
                  .WithMany(p => p.Transaction)
                  .HasForeignKey(d => d.TransactionPartyId)
                  .OnDelete(DeleteBehavior.SetNull)
                  .HasConstraintName("Transaction_transactionPartyId_fkey");

        entity.HasOne(d => d.User)
                  .WithMany(p => p.Transaction)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("Transaction_userId_fkey");
      });

      modelBuilder.Entity<TransactionParty>(entity =>
      {
        entity.Property(e => e.TransactionPartyId)
                  .HasColumnName("transactionPartyId")
                  .HasDefaultValueSql("uuid_generate_v4()");

        entity.Property(e => e.CreatedOn)
                  .HasColumnName("createdOn")
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.DefaultCategoryId).HasColumnName("defaultCategoryId");

        entity.Property(e => e.ModifiedOn)
                  .HasColumnName("modifiedOn")
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.TransactionPartyName)
                  .IsRequired()
                  .HasColumnName("transactionPartyName")
                  .HasColumnType("character varying(25)");

        entity.Property(e => e.UserId).HasColumnName("userId");

        entity.HasOne(d => d.DefaultCategory)
                  .WithMany(p => p.TransactionParty)
                  .HasForeignKey(d => d.DefaultCategoryId)
                  .OnDelete(DeleteBehavior.SetNull)
                  .HasConstraintName("TransactionParty_defaultCategoryId_fkey");

        entity.HasOne(d => d.User)
                  .WithMany(p => p.TransactionParty)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.SetNull)
                  .HasConstraintName("TransactionParty_userId_fkey");
      });

      modelBuilder.Entity<User>(entity =>
      {
        entity.HasIndex(e => e.Email)
                  .HasName("User_email_key")
                  .IsUnique();
                  
        entity.Property(e => e.UserId)
                  .HasColumnName("userId")
                  .HasDefaultValueSql("uuid_generate_v4()");

        entity.Property(e => e.ConfirmedEmail)
                  .HasColumnName("confirmedEmail")
                  .HasDefaultValueSql("false");

        entity.Property(e => e.CreatedOn)
                  .HasColumnName("createdOn")
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.Email)
                  .IsRequired()
                  .HasColumnName("email")
                  .HasColumnType("character varying(50)");

        entity.Property(e => e.FirstName)
                  .HasColumnName("firstName")
                  .HasColumnType("character varying(50)");

        entity.Property(e => e.IsRegistered)
                  .HasColumnName("isRegistered")
                  .HasDefaultValueSql("true");

        entity.Property(e => e.LastLogin)
                  .HasColumnName("lastLogin")
                  .HasColumnType("timestamp with time zone");

        entity.Property(e => e.LastName)
                  .HasColumnName("lastName")
                  .HasColumnType("character varying(50)");

        entity.Property(e => e.ModifiedOn)
                  .HasColumnName("modifiedOn")
                  .HasColumnType("timestamp with time zone")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

        entity.Property(e => e.PasswordHash).HasColumnName("passwordHash");

        entity.Property(e => e.PasswordSalt).HasColumnName("passwordSalt");
      });
    }
  }
}
