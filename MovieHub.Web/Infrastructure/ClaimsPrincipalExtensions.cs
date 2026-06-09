using System.Security.Claims;

namespace MovieHub.Web.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole(GlobalConstants.AdministratorRoleName);
    }
}
