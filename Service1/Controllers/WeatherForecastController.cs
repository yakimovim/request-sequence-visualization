using Microsoft.AspNetCore.Mvc;
using Service1.Clients;

namespace Service1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly IService2Client _service2Client;
        private readonly IService3Client _service3Client;
        private readonly IExternalServiceClient _externalServiceClient;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            IService2Client service2Client,
            IService3Client service3Client,
            IExternalServiceClient externalServiceClient,
            ILogger<WeatherForecastController> logger)
        {
            _service2Client = service2Client ?? throw new ArgumentNullException(nameof(service2Client));
            _service3Client = service3Client ?? throw new ArgumentNullException(nameof(service3Client));
            _externalServiceClient = externalServiceClient ?? throw new ArgumentNullException(nameof(externalServiceClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogInformation("Get weather forecast");

            Enumerable.Range(1, 4).ToList()
                .ForEach(_ => _logger.LogInformation("Some random message"));

            await Task.WhenAll(Enumerable.Range(1, 3).Select(_ => _service2Client.Get()));

            await _service3Client.Get();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("date")]
        public async Task<string> GetDate()
        {
            _logger.LogInformation("Get date");

            await _externalServiceClient.Get();

            return DateTime.Today.ToLongDateString();
        }
    }
}