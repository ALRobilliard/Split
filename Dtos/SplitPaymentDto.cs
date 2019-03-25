using System;

namespace SplitApi.Dtos
{
  public class SplitPaymentDto
  {
    public Guid SplitPaymentId { get; set; }
    public Guid TransactionId { get; set; }
    public Guid PayeeId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
  }
}