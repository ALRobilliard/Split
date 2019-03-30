using System;
using System.Collections.Generic;

namespace Split.Models
{
  public partial class SplitPayment
  {
    public Guid SplitPaymentId { get; set; }
    public Guid TransactionId { get; set; }
    public Guid PayeeId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public User Payee { get; set; }
    public Transaction Transaction { get; set; }
  }
}
