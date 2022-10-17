namespace Service3.Clients;

public interface IService1Client
{
    Task Get();
}

public class Service1Client : IService1Client
{
    private readonly HttpClient _client;

    public Service1Client(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task Get()
    {
        _ = await _client.GetAsync("http://localhost:5222/weatherforecast/date");
    }
}