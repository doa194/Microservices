namespace ASPNetCore.Traceability.NLog.Middleware
{
    public class CorrelationIdMiddleware(RequestDelegate next)
    {
        const string CorrelationIdHeader = "X-Correlation-ID";
        
        public async Task InvokeAsync(HttpContext context, ILogger<CorrelationIdMiddleware> logger)
        {
            string correlationId = Guid.NewGuid().ToString();

            // Check if the request already has a correlation ID
            if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var _correlationId))
            {
                context.Request.Headers[CorrelationIdHeader] = correlationId;
            }
            // Set the correlation ID in the response headers
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[CorrelationIdHeader] = correlationId;
                return Task.CompletedTask;
            });

            context.Items[CorrelationIdHeader] = correlationId;
            await next(context);
        }

        
    }
}
