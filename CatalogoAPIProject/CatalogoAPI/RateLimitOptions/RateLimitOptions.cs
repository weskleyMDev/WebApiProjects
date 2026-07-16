namespace CatalogoAPI.RateLimitOptions;

public class RateLimitOptions
{
    public const string MyRateLimiting = "MyRateLimiting";
    public int PermitLimit { get; set; } = 2;
    public int Window { get; set; } = 5;
    public int ReplenishmentPeriod { get; set; } = 1;
    public int QueueLimit { get; set; } = 2;
    public int SegmentsPerWindow { get; set; } = 2;
    public int TokenLimit { get; set; } = 2;
    public int TokenLimit2 { get; set; } = 3;
    public int TokensPerPeriod { get; set; } = 3;
    public bool AutoReplenishment { get; set; } = false;
}