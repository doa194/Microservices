using Polly;
using Polly.Retry;

namespace Microservices.Resiliency.Retry
{
    public static class RetryPolicy
    {
        public static readonly AsyncRetryPolicy<HttpResponseMessage> policy;

        static RetryPolicy()
        {
            policy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    retryCount: 1,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), 
                    onRetry: (response, timespan, retryCount, context) =>
                    {
                        // Log the retry attempt
                        Console.WriteLine($"Retry {retryCount} after {timespan.TotalSeconds} seconds due to {response.Result.StatusCode}");
                    });
        }
    }
}
