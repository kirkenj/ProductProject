namespace Infrastructure.TockenTractker
{
    public class TokenTrackingSettings
    {
        public double DurationInMinutes { get; set; }
        public string CacheSeed { get; set; } = null!;
    }
}