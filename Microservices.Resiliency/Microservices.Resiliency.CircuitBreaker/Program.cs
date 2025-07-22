using Microservices.Resiliency.CircuitBreaker;
using Polly.CircuitBreaker;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient("TargetService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7121");
});

var app = builder.Build();


app.MapGet("/circuit-breaker", static async (IHttpClientFactory httpClientFactory) =>
    {
        var client = httpClientFactory.CreateClient("TargetService");
        try
        {
            var circuitBreakerPolicy = CircuitBreakerPolicyConfig.CreateCircuitBreakerPolicy();
            var response = await circuitBreakerPolicy.ExecuteAsync(async () => await client.GetAsync("/circuit-breaker"));
            return Results.Ok(await response.Content.ReadAsStringAsync());
        }
        catch (BrokenCircuitException)
        {
            Console.WriteLine("Circuit is open, request cannot be processed.");
            return Results.Problem("Service is currently unavailable due to circuit breaker being open.");
        }
        catch (Exception)
        {
            throw;
        }
});

app.Run();