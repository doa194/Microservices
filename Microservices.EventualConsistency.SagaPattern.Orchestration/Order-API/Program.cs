using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order_API.Consumers;
using Order_API.Context;
using Order_API.Models;
using Order_API.Models.VM;
using Shared.Configurations;
using Shared.Events; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CompletedOrderEventConsumer>();
    x.AddConsumer<FailedOrderEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ"]);
        cfg.ReceiveEndpoint(RabbitMQConfig.OrderToOrderCompletedEventQueue, e => e.ConfigureConsumer<CompletedOrderEventConsumer>(context));
        cfg.ReceiveEndpoint(RabbitMQConfig.OrderToOrderFailedEventQueue, e => e.ConfigureConsumer<FailedOrderEventConsumer>(context));
    });
});

builder.Services.AddDbContext<Order_API.Context.Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapPost("order-create", async (OrderCreateVM data, Context context, ISendEndpointProvider sendEndpoint) => {
    Order order = new Order()
    {
        CustomerId = data.CustomerId,
        OrderDate = DateTime.UtcNow,
        OrderStatus = Order_API.Enums.OrderStatusEnum.Pending,
        OrderDetails = data.OrderDetailsVM.Select(od => new OrderDetails
        {
            ProductId = od.ProductId,
            Count = od.Count,
            Price = od.Price
        }).ToList()
    };
    await context.Orders.AddAsync(order);
    await context.SaveChangesAsync();

    // Publish the event to RabbitMQ
    StartOrderEvent startOrderEvent = new StartOrderEvent
    {
        CustomerId = order.CustomerId,
        OrderId = order.OrderId,
        TotalAmount = order.OrderDetails.Sum(od => od.Price * od.Count),
    };

    // Get the send endpoint for the state machine queue and send the StartOrderEvent
    var endpoint = await sendEndpoint.GetSendEndpoint(new Uri($"queue:{RabbitMQConfig.StateMachineQueue}"));
    await endpoint.Send<StartOrderEvent>(startOrderEvent);
});

app.UseHttpsRedirection();

app.Run();
