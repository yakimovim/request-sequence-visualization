namespace Common;

public class PreviousServiceProvider
{
    private static readonly AsyncLocal<string?> Value = new();

    public string? GetPreviousService() => Value.Value;

    public void SetPreviousService(string? value)
    {
        Value.Value = value;
    }
}