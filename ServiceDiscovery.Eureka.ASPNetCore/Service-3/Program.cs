var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddConfigurationDiscoveryClient(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.


app.MapGet("/", () =>
{

});

app.Run();

