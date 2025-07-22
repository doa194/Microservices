var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("TargetService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7121");
});

var app = builder.Build();

app.MapGet("/circuit-breaker", () => {

    Console.WriteLine($"Processing request...{myStruct.myField++}");
    throw new Exception("This is a simulated exception to demonstrate circuit breaker functionality.");
});

app.MapGet("/retry", () => {
    Console.WriteLine($"Processing request...{myStruct.myField++}");
    throw new Exception("This is a simulated exception to demonstrate retry functionality.");
});

app.MapGet("/timeout", async () => {
    await Task.Delay(10000); // Simulate a long-running operation
    Console.WriteLine($"Processing request...{myStruct.myField++}");
});

app.MapGet("/fallback", async () => {
    throw new Exception("This is a simulated exception to demonstrate fallback functionality.");
});


app.Run();


class myStruct
{
    public static int myField = 0;
};