using System.Security.Claims;

namespace ProjectClock.MVC.Extensions
{
    public static class ClaimsExtensions
    {
        public static bool TryGetAuthenticatedUserId(this IEnumerable<Claim> claims, out int userId)
        {
            var userIdText = claims
                .Where(c => c.Type == "UserId")
                .Select(c => c.Value)
                .FirstOrDefault();

            return Int32.TryParse(userIdText, out userId);
        }
    }
}
