var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/ready", () => "");
app.MapGet("/commit", () => "");
app.MapGet("/rollback", () => "");

app.Run();