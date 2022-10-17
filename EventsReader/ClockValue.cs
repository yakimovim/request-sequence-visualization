namespace EventsReader;

public class ClockValue : IComparable<ClockValue>
{
    private readonly int[] _values;

    private ClockValue(int[] values)
    {
        _values = values ?? throw new ArgumentNullException(nameof(values));
    }

    public static ClockValue Parse(string clock)
    {
        var parts = clock.Split('.', StringSplitOptions.RemoveEmptyEntries);

        var values = parts.Select(int.Parse).ToArray();

        return new ClockValue(values);
    }

    public int CompareTo(ClockValue? other)
    {
        if(other == null) return 1;

        return 1;
    }
}