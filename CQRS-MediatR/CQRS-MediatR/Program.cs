using CQRS_Manual.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Context).Assembly));
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
