using System;

namespace SplitApi.Dtos
{
  public class CategoryDto
  {
    public Guid CategoryId { get; set; }
    public String CategoryName { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public Guid UserId { get; set; }
    public int CategoryType { get; set; }
  }
}