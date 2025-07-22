using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();

builder.Services.AddReverseProxy()
    .LoadFromMemory(new List<RouteConfig>
    {
        new RouteConfig
        {
            RouteId = "API1-Route",
            ClusterId = "API1-Cluster",
            Match = new RouteMatch
            {
                Path = "/api1/{**catch-all}"
            },
            Transforms = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "PathRemovePrefix", "/api1" }
                }
            }
        },
        new RouteConfig
        {
            RouteId = "API2-Route",
            ClusterId = "API2-Cluster",
            Match = new RouteMatch
            {
                Path = "/api2/{**catch-all}"
            },
            Transforms = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "PathRemovePrefix", "/api2" }
                }
            }
        },
        new RouteConfig
        {
            RouteId = "APU3-Route",
            ClusterId = "API3-Cluster",
            Match = new RouteMatch
            {
                Path = "/api3/{**catch-all}"
            },
            Transforms = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "PathRemovePrefix", "/api3" }
                }
            }
        }
    }, 
    
    new List<ClusterConfig>
    {
        new ClusterConfig
        {
            ClusterId = "API1-Cluster",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "destination1", new DestinationConfig { Address = "https://localhost:7175/" } }
            }
        },
        new ClusterConfig
        {
            ClusterId = "API2-Cluster",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "destination1", new DestinationConfig { Address = "https://localhost:7232/" } }
            }
        },
        new ClusterConfig
        {
            ClusterId = "API3-Cluster",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "destination1", new DestinationConfig { Address = "https://localhost:7248/" } }
            }
        }
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapReverseProxy();

app.Run();
