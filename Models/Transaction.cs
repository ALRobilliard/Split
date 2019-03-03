using System;
using System.Collections.Generic;

namespace SplitApi.Models
{
  public partial class Transaction
  {
    public Transaction()
    {
      SplitPayment = new HashSet<SplitPayment>();
    }

    public Guid TransactionId { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? TransactionPartyId { get; set; }
    public Guid? AccountInId { get; set; }
    public Guid? AccountOutId { get; set; }
    public decimal? Amount { get; set; }
    public bool IsShared { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public Guid? UserId { get; set; }

    public Account AccountIn { get; set; }
    public Account AccountOut { get; set; }
    public Category Category { get; set; }
    public TransactionParty TransactionParty { get; set; }
    public User User { get; set; }
    public ICollection<SplitPayment> SplitPayment { get; set; }
  }
}
