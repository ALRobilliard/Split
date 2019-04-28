using System;
using System.Collections.Generic;

namespace Split.Models
{
  public partial class Account
  {
    public Account()
    {
      TransactionAccountIn = new HashSet<Transaction>();
      TransactionAccountOut = new HashSet<Transaction>();
    }

    public Guid AccountId { get; set; }
    public string AccountName { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public int AccountType { get; set; }
    public decimal? Balance { get; set; }
    public decimal? Limit { get; set; }

    public User User { get; set; }
    public ICollection<Transaction> TransactionAccountIn { get; set; }
    public ICollection<Transaction> TransactionAccountOut { get; set; }
  }

  public enum AccountType
  {
    Debit,
    Credit
  }
}
