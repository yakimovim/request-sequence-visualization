namespace Service1.Clients;

public interface IExternalServiceClient
{
    Task Get();
}

public class ExternalServiceClient : IExternalServiceClient
{
    private readonly HttpClient _client;

    public ExternalServiceClient(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task Get()
    {
        _ = await _client.GetAsync("http://localhost:5255/weatherforecast");
    }
}