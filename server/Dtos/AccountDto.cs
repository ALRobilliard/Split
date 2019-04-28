using System;

namespace Split.Dtos
{
  public class AccountDto
  {
    public Guid? AccountId { get; set; }
    public string AccountName { get; set; }
    public int? AccountType { get; set; }
    public decimal? Balance { get; set; }
    public DateTime? CreatedOn { get; set; }
    public decimal? Limit { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public Guid UserId { get; set; }
  }
}