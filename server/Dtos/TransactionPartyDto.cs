using System;

namespace Split.Dtos
{
  public class TransactionPartyDto
  {
    public Guid TransactionPartyId { get; set; }
    public string TransactionPartyName { get; set; }
    public Guid? DefaultCategoryId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public Guid? UserId { get; set; }
  }
}