using Microsoft.AspNetCore.Builder;

namespace Common
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
        {
            if(app == null)
                throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<LoggingMiddleware>();
        }
    }
}