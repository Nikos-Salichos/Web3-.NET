namespace Domain.Models
{
    public class RateLimitOptions
    {
        public bool AutoReplenishment { get; set; }
        public int PermitLimit { get; set; }
        public int Window { get; set; }
        public int QueueLimit { get; set; }
    }
}
