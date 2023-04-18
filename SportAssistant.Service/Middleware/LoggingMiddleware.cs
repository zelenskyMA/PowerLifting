using SportAssistant.Service.Extensions;
using System.Text;

namespace SportAssistant.Service.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // var originalResponseBody = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            // context.Response.Body = responseBody;
            await _next.Invoke(context);

            if (CheckPath(context))
            {
                await LogRequest(context);
                await LogResponse(context, responseBody, context.Response.Body);
            }
        }
    }

    private async Task LogRequest(HttpContext context)
    {
        var requestContent = new StringBuilder();

        requestContent.AppendLine("=== Request Info ===");
        requestContent.AppendLine($"method = {context.Request.Method.ToUpper()}");
        requestContent.AppendLine($"path = {context.Request.Path}");

        requestContent.AppendLine("-- headers");
        foreach (var (headerKey, headerValue) in context.Request.Headers)
        {
            requestContent.AppendLine($"header = {headerKey}    value = {headerValue}");
        }

        requestContent.AppendLine("-- body");
        context.Request.EnableBuffering();
        var requestReader = new StreamReader(context.Request.Body);
        var content = await requestReader.ReadToEndAsync();
        requestContent.AppendLine($"body = {content}");

        _logger.LogInformation(requestContent.ToString());
        context.Request.Body.Position = 0;
    }

    private async Task LogResponse(HttpContext context, MemoryStream responseBody, Stream originalResponseBody)
    {
        var responseContent = new StringBuilder();
        responseContent.AppendLine("=== Response Info ===");

        responseContent.AppendLine("-- headers");
        foreach (var (headerKey, headerValue) in context.Response.Headers)
        {
            responseContent.AppendLine($"header = {headerKey}    value = {headerValue}");
        }

        responseContent.AppendLine("-- body");
        responseBody.Position = 0;
        var content = await new StreamReader(responseBody).ReadToEndAsync();
        responseContent.AppendLine($"body = {content}");
        responseBody.Position = 0;
        await responseBody.CopyToAsync(originalResponseBody);
        context.Response.Body = originalResponseBody;

        _logger.LogInformation(responseContent.ToString());
    }

    private bool CheckPath(HttpContext context)
    {
        var path = context.Request.Path;
        var endpoint = context.GetEndpoint();
        var attribute = endpoint?.Metadata.GetMetadata<LogItemAttribute>();

        return attribute != null;
    }

}