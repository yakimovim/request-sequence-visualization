namespace Service1.Clients;

public interface IService3Client
{
    Task Get();
}

public class Service3Client : IService3Client
{
    private readonly HttpClient _client;

    public Service3Client(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task Get()
    {
        _ = await _client.GetAsync("http://localhost:5056/weatherforecast");
    }
}