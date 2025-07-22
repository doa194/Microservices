using ASPNetCore.Traceability.NLog.Middleware;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Logging.ClearProviders();
builder.Logging.AddNLog("nlog.config"); // Specify the NLog configuration file


app.UseMiddleware<CorrelationIdMiddleware>();

app.MapGet("/", (HttpContext context, ILogger<Program> logger) => 
{
    var correlationId = context.Request.Headers["X-Correlation-ID"].ToString();

    correlationId = context.Items["X-Correlation-ID"]?.ToString() ?? correlationId;

    NLog.MappedDiagnosticsContext.Set("CorrelationId", correlationId);
    logger.LogDebug("This is a debug message with correlation ID: {CorrelationId}", correlationId);
});

app.Run();
