namespace Application.Models.CacheKeyGenerator
{
    public static class CacheKeyGenerator
    {
        public static string KeyForEmailChangeTokenCaching(string newUserEmail) => $"Email change {newUserEmail}";
    }
}
