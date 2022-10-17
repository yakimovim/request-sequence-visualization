namespace Service1.Clients;

public interface IService2Client
{
    Task Get();
}

public class Service2Client : IService2Client
{
    private readonly HttpClient _client;

    public Service2Client(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task Get()
    {
        _ = await _client.GetAsync("http://localhost:5106/weatherforecast");
    }
}