using Polly;
using Polly.CircuitBreaker;

namespace Microservices.Resiliency.CircuitBreaker
{
    public class CircuitBreakerPolicyConfig
    {
        public static AsyncCircuitBreakerPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy()
        {
            return Policy
                .HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 1,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (outcome, timespan) =>
                    {
                        Console.WriteLine($"Circuit broken! Will break for {timespan.TotalSeconds} seconds.");
                    },
                    onReset: () =>
                    {
                        Console.WriteLine("Circuit reset!");
                    },
                    onHalfOpen: () =>
                    {
                        Console.WriteLine("Circuit is half-open, next call will test the circuit.");
                    });
        }
    }
}
