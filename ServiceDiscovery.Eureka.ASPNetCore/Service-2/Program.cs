using Steeltoe.Common.Discovery;
using Steeltoe.Discovery;
using Steeltoe.Discovery.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDiscoveryClient(builder.Configuration);

var app = builder.Build();

app.MapGet("/", async (IHttpClientFactory clientFactory) =>
{
    var httpClient = clientFactory.CreateClient("Service3Client");
    return await httpClient.GetStringAsync("/");
});

app.Run();
