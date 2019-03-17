using System;

namespace SplitApi.Dtos
{
  public class AccountDto
  {
    public Guid? AccountId { get; set; }
    public string AccountName { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
  }
}