namespace Common;

public class InitialServiceProvider
{
    private static readonly AsyncLocal<string> Value = new();

    public string GetInitialService()
    {
        var value = Value.Value;

        if (string.IsNullOrWhiteSpace(value))
        {
            value = ServiceNameProvider.ServiceName;
            SetInitialService(value);
        }

        return value;
    }

    public void SetInitialService(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

        Value.Value = value;
    }
}