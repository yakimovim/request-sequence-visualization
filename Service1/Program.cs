using Common;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Service1.Clients;

ServiceNameProvider.ServiceName = "Service1";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .MinimumLevel.Override("System", LogEventLevel.Error)
    .Enrich.FromLogContext()
    .WriteTo.Console(new CompactJsonFormatter())
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddLoggingServices();

builder.Services.AddHttpClientWithHeaders<IService2Client, Service2Client>();
builder.Services.AddHttpClientWithHeaders<IService3Client, Service3Client>();
builder.Services.AddHttpClientWithHeaders<IExternalServiceClient, ExternalServiceClient>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
