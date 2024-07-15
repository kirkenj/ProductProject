using System.Security.Claims;

namespace AuthAPI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal User) => Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
        public static string GetUserLogin(this ClaimsPrincipal User) => User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        public static string GetUserRole(this ClaimsPrincipal User) => User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
        public static string GetUserEmail(this ClaimsPrincipal User) => User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
    }
}
