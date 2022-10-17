using Microsoft.AspNetCore.Mvc;
using Service2.Clients;

namespace Service2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly IService3Client _service3Client;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            IService3Client service3Client,
            ILogger<WeatherForecastController> logger)
        {
            _service3Client = service3Client ?? throw new ArgumentNullException(nameof(service3Client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogInformation("Get weather forecast");

            Enumerable.Range(1, 3).ToList()
                .ForEach(_ => _logger.LogInformation("Some random message"));

            await _service3Client.Get();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}