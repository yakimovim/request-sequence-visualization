using Microsoft.AspNetCore.Mvc;
using Service3.Clients;

namespace Service3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly IService1Client _service1Client;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            IService1Client service1Client,
            ILogger<WeatherForecastController> logger)
        {
            _service1Client = service1Client ?? throw new ArgumentNullException(nameof(service1Client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogInformation("Get weather forecast");

            Enumerable.Range(1, 2).ToList()
                .ForEach(_ => _logger.LogInformation("Some random message"));

            await _service1Client.Get();

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