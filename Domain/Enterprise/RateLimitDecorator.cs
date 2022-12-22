namespace Domain.Enterprise
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RateLimitDecorator : Attribute
    {
        public RateLimitType RateLimitType { get; set; }
    }
}
