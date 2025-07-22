var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient("TargetService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7121");
});

var app = builder.Build();

app.MapGet("/timeout", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("TargetService");
    try
    {
        var timeoutPolicy = Microservices.Resiliency.Timeout.TimeoutPolicyConfig.policy;  
        var response = await timeoutPolicy.ExecuteAsync(async () => await client.GetAsync("/timeout"));
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while calling the target service.");
    }
});

app.Run();
