using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Common;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly CorrelationIdProvider _correlationIdProvider;
    private readonly InitialServiceProvider _initialServiceProvider;
    private readonly PreviousServiceProvider _previousServiceProvider;
    private readonly RequestClockProvider _requestClockProvider;

    public LoggingMiddleware(
        RequestDelegate next,
        CorrelationIdProvider correlationIdProvider,
        InitialServiceProvider initialServiceProvider,
        PreviousServiceProvider previousServiceProvider,
        RequestClockProvider requestClockProvider
        )
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _correlationIdProvider = correlationIdProvider ?? throw new ArgumentNullException(nameof(correlationIdProvider));
        _initialServiceProvider = initialServiceProvider ?? throw new ArgumentNullException(nameof(initialServiceProvider));
        _previousServiceProvider = previousServiceProvider ?? throw new ArgumentNullException(nameof(previousServiceProvider));
        _requestClockProvider = requestClockProvider ?? throw new ArgumentNullException(nameof(requestClockProvider));
    }

    public async Task Invoke(HttpContext context)
    {
        GetCorrelationId(context);
        GetInitialsService(context);
        GetPreviousService(context);
        GetPreviousClock(context);

        using (LogContext.PushProperty(Names.CurrentServiceName, ServiceNameProvider.ServiceName))
        using (LogContext.PushProperty(Names.CorrelationIdHeaderName, _correlationIdProvider.GetCorrelationId()))
        using (LogContext.PushProperty(Names.InitialServiceHeaderName, _initialServiceProvider.GetInitialService()))
        using (LogContext.PushProperty(Names.PreviousServiceHeaderName, _previousServiceProvider.GetPreviousService()))
        using (LogContext.PushProperty(Names.RequestClockHeaderName, _requestClockProvider.GetPreviousServiceClock()))
        {
            await _next(context);
        }
    }

    private void GetCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey(Names.CorrelationIdHeaderName)
            && context.Request.Headers[Names.CorrelationIdHeaderName].Any())
        {
            _correlationIdProvider.SetCorrelationId(context.Request.Headers[Names.CorrelationIdHeaderName][0]);
        }
        else
        {
            _correlationIdProvider.SetCorrelationId(Guid.NewGuid().ToString("N"));
        }
    }

    private void GetPreviousService(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey(Names.PreviousServiceHeaderName)
            && context.Request.Headers[Names.PreviousServiceHeaderName].Any())
        {
            _previousServiceProvider.SetPreviousService(context.Request.Headers[Names.PreviousServiceHeaderName][0]);
        }
        else
        {
            _previousServiceProvider.SetPreviousService(null);
        }
    }

    private void GetPreviousClock(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey(Names.RequestClockHeaderName)
            && context.Request.Headers[Names.RequestClockHeaderName].Any())
        {
            _requestClockProvider.SetPreviousServiceClock(context.Request.Headers[Names.RequestClockHeaderName][0]);
        }
        else
        {
            _requestClockProvider.SetPreviousServiceClock(string.Empty);
        }
    }

    private void GetInitialsService(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey(Names.InitialServiceHeaderName)
            && context.Request.Headers[Names.InitialServiceHeaderName].Any())
        {
            _initialServiceProvider.SetInitialService(context.Request.Headers[Names.InitialServiceHeaderName][0]);
        }
        else
        {
            _initialServiceProvider.SetInitialService(ServiceNameProvider.ServiceName);
        }
    }
}