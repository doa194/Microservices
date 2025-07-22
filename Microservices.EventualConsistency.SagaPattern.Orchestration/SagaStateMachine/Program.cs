using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaStateMachine.StateDbContext;
using SagaStateMachine.StateInstance;
using SagaStateMachine.StateMachine;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
        .EntityFrameworkRepository(cfg =>
        {
            cfg.ConcurrencyMode = ConcurrencyMode.Optimistic;
            cfg.AddDbContext<DbContext, OrderStateDBContext>((provider, options) =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
            });
        });

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ"]);
    });
});


var host = builder.Build();
host.Run();
