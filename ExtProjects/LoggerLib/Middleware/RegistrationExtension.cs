using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace LoggerLib.Middleware;

/// <summary>
/// Регистрация DI
/// </summary>
public static class RegistrationExtension
{
    public static WebApplicationBuilder RegisterLogger(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders().SetMinimumLevel(LogLevel.Trace);
        builder.Host.UseNLog();

        NLog.LogManager.Configuration = new NLogLoggingConfiguration(builder.Configuration.GetSection("NLog"));

        return builder;
    }
}
