using System;

namespace Split.Dtos
{
  public class TransactionDto
  {
    public Guid TransactionId { get; set; }
    public Guid? CategoryId { get; set; }
    public string CategoryName { get; set; }
    public Guid? TransactionPartyId { get; set; }
    public string TransactionPartyName { get; set; }
    public Guid? AccountInId { get; set; }
    public string AccountInName { get; set; }
    public Guid? AccountOutId { get; set; }
    public string AccountOutName { get; set; }
    public decimal? Amount { get; set; }
    public bool IsShared { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public Guid? UserId { get; set; }
  }
}