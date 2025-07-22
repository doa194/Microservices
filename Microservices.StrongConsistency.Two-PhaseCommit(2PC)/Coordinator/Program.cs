using Coordinator.Model.Context;
using Coordinator.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<Context>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register HTTP clients
builder.Services.AddHttpClient("Order-Service", client =>
{
    client.BaseAddress = new Uri("https://localhost:7113");
});

builder.Services.AddHttpClient("Payment-Service", client =>
{
    client.BaseAddress = new Uri("https://localhost:7013");
});

builder.Services.AddHttpClient("Stock-Service", client =>
{
    client.BaseAddress = new Uri("https://localhost:7173");
});

builder.Services.AddSingleton<ITransaction, Coordinator.Transaction.Transaction>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/transaction", async (ITransaction transaction) =>
{
    var id = await transaction.CreateTransactionAsync();
    await transaction.PrepareServiceAsync(id);
    bool state = await transaction.CheckServiceAsync(id);

    if (state)
    {
        await transaction.CommitTransactionAsync(id);
        state = await transaction.TransactionStatusAsync(id);
    }
    if(!state)
    {
        await transaction.TransactionRollbackAsync(id);
    }
});

app.UseHttpsRedirection();

app.Run();
