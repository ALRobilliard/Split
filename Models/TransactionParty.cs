using System;
using System.Collections.Generic;

namespace SplitApi.Models
{
  public partial class TransactionParty
  {
    public TransactionParty()
    {
      Transaction = new HashSet<Transaction>();
    }

    public Guid TransactionPartyId { get; set; }
    public string TransactionPartyName { get; set; }
    public Guid? DefaultCategoryId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public Category DefaultCategory { get; set; }
    public ICollection<Transaction> Transaction { get; set; }
  }
}
