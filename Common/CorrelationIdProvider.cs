namespace Common;

public class CorrelationIdProvider
{
    private static readonly AsyncLocal<string> Value = new();

    public string GetCorrelationId()
    {
        var value = Value.Value;

        if (string.IsNullOrWhiteSpace(value))
        {
            value = Guid.NewGuid().ToString("N");
            SetCorrelationId(value);
        }

        return value;
    }

    public void SetCorrelationId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

        Value.Value = value;
    }
}