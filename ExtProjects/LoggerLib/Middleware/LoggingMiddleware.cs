using LoggerLib.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace LoggerLib.Middleware;

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
        var record = new LogRecord();

        var originalBodyStream = context.Response.Body;

        try
        {
            await ReadRequest(context, record);

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next.Invoke(context);
                
                await ReadResponse(context, record);

                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        catch (Exception ex)
        {
            context.Response.Body = originalBodyStream; // нужно для корректной отсылки ошибок через ErrorHandling middleware
            record.Error = ex.Message;
            // record.StackTrace = ex.StackTrace;

            throw;
        }
        finally
        {
            var noLoggingAttribute = context.GetEndpoint()?.Metadata.GetMetadata<ExcludeLogItemAttribute>();
            SetCallerData(context, record);

            if (record.CallerId > 0 && noLoggingAttribute == null)
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = false
                };

                _logger.LogInformation(JsonSerializer.Serialize(record, options));
            }
        }
    }

    private async Task ReadRequest(HttpContext context, LogRecord record)
    {
        record.Path = context.Request.Path;
        record.Method = context.Request.Method;
        HttpRequest request = context.Request;

        try
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            record.Request = Encoding.UTF8.GetString(buffer);
        }
        finally
        {
            request.Body.Position = 0;
        }
    }

    private async Task ReadResponse(HttpContext context, LogRecord record)
    {
        try
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            record.Response = await new StreamReader(context.Response.Body).ReadToEndAsync();
        }
        finally
        {
            //We need to reset the reader for the response so that the client can read it.
            context.Response.Body.Seek(0, SeekOrigin.Begin);
        }
    }

    private void SetCallerData(HttpContext context, LogRecord record)
    {
        var userIdString = context?.User?.Claims?.FirstOrDefault(i => i.Type == ClaimTypes.Sid)?.Value;
        if (int.TryParse(userIdString, out int userId))
        {
            record.CallerId = userId;
        }
    }
}