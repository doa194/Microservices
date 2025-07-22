using Polly.Retry;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient("TargetService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7121");
});

var app = builder.Build();

app.MapGet("/retry", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("TargetService");
    try
    {
        var retryPolicy = Microservices.Resiliency.Retry.RetryPolicy.policy;
        var response = await retryPolicy.ExecuteAsync(async () => await client.GetAsync("/retry"));
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while calling the target service.");
    }
});


app.Run();
