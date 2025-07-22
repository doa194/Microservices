using Polly;
using Polly.Timeout;

namespace Microservices.Resiliency.Timeout
{
    public static class TimeoutPolicyConfig
    {
        public static readonly AsyncTimeoutPolicy<HttpResponseMessage> policy;
        static TimeoutPolicyConfig()
        {
            policy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(5), TimeoutStrategy.Pessimistic, onTimeoutAsync: (context, timespan, task) =>
            {
                Console.WriteLine($"Timeout occurred after {timespan.TotalSeconds} seconds.");
                return Task.CompletedTask;
            });
        }
    }
}
