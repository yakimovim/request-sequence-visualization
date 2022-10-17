using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Common;

public class RequestHandler : DelegatingHandler
{
    private readonly ILogger<RequestHandler> _logger;
    private readonly CorrelationIdProvider _correlationIdProvider;
    private readonly InitialServiceProvider _initialServiceProvider;
    private readonly RequestClockProvider _requestClockProvider;

    public RequestHandler(
        ILogger<RequestHandler> logger,
        CorrelationIdProvider correlationIdProvider,
        InitialServiceProvider initialServiceProvider,
        RequestClockProvider requestClockProvider
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _correlationIdProvider = correlationIdProvider ?? throw new ArgumentNullException(nameof(correlationIdProvider));
        _initialServiceProvider = initialServiceProvider ?? throw new ArgumentNullException(nameof(initialServiceProvider));
        _requestClockProvider = requestClockProvider ?? throw new ArgumentNullException(nameof(requestClockProvider));
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var requestClockValue = _requestClockProvider.GetNextCurrentServiceClock();

        request.Headers.Add(Names.CorrelationIdHeaderName, _correlationIdProvider.GetCorrelationId());
        request.Headers.Add(Names.InitialServiceHeaderName, _initialServiceProvider.GetInitialService());
        request.Headers.Add(Names.PreviousServiceHeaderName, ServiceNameProvider.ServiceName);
        request.Headers.Add(Names.RequestClockHeaderName, requestClockValue);

        using (LogContext.PushProperty(Names.RequestBoundaryForName, requestClockValue))
        using (LogContext.PushProperty(Names.RequestURLName, $"{request.Method} {request.RequestUri}"))
        {
            _logger.LogInformation("Sending request...");

            return base.SendAsync(request, cancellationToken);
        }
    }

}