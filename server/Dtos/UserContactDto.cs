using System;

namespace Split.Dtos
{
  public class UserContactDto
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid ContactId { get; set; }
    public string ContactName { get; set; }
    public string ContactEmail { get; set; }
    public Guid UserId { get; set; }
  }
}