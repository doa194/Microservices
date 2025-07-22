using EventHandlerService;
using EventHandlerService.Services;
using Shared.Services;
using Shared.Services.Abstraction;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<EventStoreBGService>();
builder.Services.AddSingleton<IEventStoreService, EventStoreService>();
builder.Services.AddSingleton<IMongoDBService, MongoDBService>();

var host = builder.Build();
host.Run();
