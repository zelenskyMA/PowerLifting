using SportAssistant.Domain.CustomExceptions;

namespace SportAssistant.Service.Middleware
{

    internal static class ErrorHandlingExtensions
    {
        internal static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            return app.Use((ctx, next) => HandleErrorAsync(ctx, next, env));
        }

        private static async Task HandleErrorAsync(HttpContext context, Func<Task> next, IWebHostEnvironment env)
        {
            try
            {
                if (next != null)
                {
                    await next();
                }
            }

            catch (Exception e)
            {
                if (e is OutOfMemoryException)
                {
                    GC.Collect();
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = e switch
                {
                    /* BadRequestException => StatusCodes.Status400BadRequest,                     
                     ForbiddenException => StatusCodes.Status403Forbidden,
                     NotFoundException => StatusCodes.Status404NotFound,
                     ConflictException => StatusCodes.Status409Conflict,
                     GoneException => StatusCodes.Status410Gone,*/

                    UnauthorizedException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError,
                };

                var error = new
                {
                    Message = e.Message,
                    Detail = e.StackTrace, // env.IsDevelopment() || env.IsEnvironment("Debug") ? e.StackTrace : null,
                    ExtData = e.Data?.Count > 0 ? e.Data : null,
                };

                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
