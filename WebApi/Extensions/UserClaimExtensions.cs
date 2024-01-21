using System.Security.Claims;

namespace WebApi.Extensions
{
    public static class UserClaimExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            string userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userId, out int result);
            return result;
        }
    }
}
