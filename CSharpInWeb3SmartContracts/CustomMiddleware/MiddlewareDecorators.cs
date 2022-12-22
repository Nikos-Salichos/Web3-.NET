namespace WebApi.CustomMiddleware
{
    public class MiddlewareDecorators
    {
        [AttributeUsage(AttributeTargets.Method)]
        public class LimitRequest : Attribute
        {
            public int TimeWindow
            {
                get;
                set;
            }
            public int MaxRequests
            {
                get;
                set;
            }
        }
    }
}
