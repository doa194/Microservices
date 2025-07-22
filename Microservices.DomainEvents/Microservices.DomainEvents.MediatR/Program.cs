using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var mediator = builder.Services.BuildServiceProvider().GetRequiredService<IMediator>();

CreateOrderEvent createOrderEvent = new CreateOrderEvent
{
    // This is a domain event that will be published
    OrderId = Guid.NewGuid(),
    CustomerName = "John Doe",
    OrderDate = DateTime.UtcNow
};
await mediator.Publish(createOrderEvent); // Publish the domain event


interface IDomainEvent : INotification
{
    // Marker interface for domain events
}

class CreateOrderEvent : IDomainEvent
{
    // This class represents a domain event for creating an order
    public Guid OrderId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
}


class CreateOrderEventHandler : INotificationHandler<CreateOrderEvent>
{
    public Task Handle(CreateOrderEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Order Created: {notification.OrderId}, Customer: {notification.CustomerName}, Date: {notification.OrderDate}");
        return Task.CompletedTask;
    }
}