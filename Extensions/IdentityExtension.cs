using System;
using System.Linq;
using System.Security.Claims;

namespace SplitApi.Extensions
{
  public static class IdentityExtension
  {
    public static Guid? GetUserId(this ClaimsIdentity claimsIdentity)
    {
      Claim userIdClaim = claimsIdentity.Claims.Where(c => c.Type.Equals(ClaimTypes.NameIdentifier)).FirstOrDefault();
      Guid? userId = null;
      if (userIdClaim != null)
      {
        userId = new Guid(userIdClaim.Value);
      }

      return userId;
    }
  }
}