using Microsoft.Extensions.DependencyInjection;

namespace Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoggingServices(this IServiceCollection services)
    {
        services.AddSingleton<CorrelationIdProvider>();
        services.AddSingleton<InitialServiceProvider>();
        services.AddSingleton<PreviousServiceProvider>();
        services.AddSingleton<RequestClockProvider>();

        services.AddScoped<RequestHandler>();

        return services;
    }

    public static IHttpClientBuilder AddHttpClientWithHeaders<TInterface, TClass>(this IServiceCollection services)
        where TInterface : class
        where TClass : class, TInterface
    {
        return services.AddHttpClient<TInterface, TClass>()
            .AddHttpMessageHandler<RequestHandler>();
    }
}