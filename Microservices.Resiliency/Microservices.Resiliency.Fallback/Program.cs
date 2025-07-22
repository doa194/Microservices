using Microservices.Resiliency.Fallback;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("TargetService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7121");
});

var app = builder.Build();

app.MapGet("/fallback", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("TargetService");
    try
    {
        var fallbackPolicy = FallbackPolicyConfig.policy;
        var response = await fallbackPolicy.ExecuteAsync(async () => await client.GetAsync("/fallback"));
        return Results.Ok(response.Content.ReadAsStringAsync().Result);
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while calling the target service.");
        throw ex;
    }
});

app.Run();
