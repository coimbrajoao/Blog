using System.Security.Claims;
using Blog.Models;

namespace Blogg.Extensions;

public static class RoleClaimsExtension
{
    public static IEnumerable<Claim> GetClaims(this User user)
    {
        var claims = new List<Claim>
       {
           new Claim(ClaimTypes.Email, user.Email)
       };
        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Slug)));

        return claims;
    }
}