using MassTransit;
using Shared.Configurations;
using Shared.Events;
using Stock_API.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<StartPaymentEventConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ"]);
        cfg.ReceiveEndpoint(RabbitMQConfig.PaymentToPaymentStartedEventQueue, e => e.ConfigureConsumer<StartPaymentEventConsumer>(context));
    });
});

var app = builder.Build();

app.Run();


