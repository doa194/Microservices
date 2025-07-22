using MassTransit;
using MongoDB.Driver;
using Shared.Configurations;
using Stock_API.Consumers;
using Stock_API.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateOrderEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ"]);
        cfg.ReceiveEndpoint(RabbitMQConfig.StockToOrderCreateEventQueue, e => e.ConfigureConsumer<CreateOrderEventConsumer>(context));
    });
});

builder.Services.AddSingleton<MongoDBContext>();

var app = builder.Build();

var mongoDB = builder.Services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<MongoDBContext>(); 

if (!await (await mongoDB.GetCollection<Stock_API.Models.Stock>().FindAsync(_ => true)).AnyAsync())
{
    await mongoDB.GetCollection<Stock_API.Models.Stock>().InsertManyAsync(new List<Stock_API.Models.Stock>
    {
        new Stock_API.Models.Stock { ProductId = 1, Count = 100 },
        new Stock_API.Models.Stock { ProductId = 2, Count = 200 },
        new Stock_API.Models.Stock { ProductId = 3, Count = 300 }
    });
}

app.Run();


