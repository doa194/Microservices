using Polly;
using Polly.Fallback;
using System.Net;

namespace Microservices.Resiliency.Fallback
{
    public static class FallbackPolicyConfig
    {
        public static readonly AsyncFallbackPolicy<HttpResponseMessage> policy;
        static FallbackPolicyConfig()
        {
            policy = Policy
                .HandleResult<HttpResponseMessage>(response => response.StatusCode == HttpStatusCode.InternalServerError)
                .FallbackAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Fallback response: Service is currently unavailable. Please try again later.")
                });
        }

    }
}
