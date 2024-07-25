namespace Application.Features.CacheKeyGenerator
{
    public static class CacheKeyGenerator
    {
        public static string KeyForRegistrationCaching(string futureUserEmail) => $"UserRegistration {futureUserEmail}";
        public static string KeyForEmailChangeTokenCaching(string newUserEmail) => $"Email change {newUserEmail}";
        public static string KeyForUserTokensInvalidationCaching(string userId) => $"User tokens invalidation {userId}";
        public static string KeyForTokenTrackingCaching(string token) => $"Token tracking {token}";
    }
}
