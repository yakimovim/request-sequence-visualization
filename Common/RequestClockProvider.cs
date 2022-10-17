namespace Common;

public class RequestClockProvider
{
    private class ClockHolder
    {
        public string PreviousServiceClock { get; init; }

        public int CurrentClock { get; set; }
    }

    private static readonly AsyncLocal<ClockHolder> Clock = new();

    public void SetPreviousServiceClock(string? value)
    {
        Clock.Value = new ClockHolder
        {
            PreviousServiceClock = value ?? string.Empty
        };
    }

    public string GetPreviousServiceClock() => Clock.Value?.PreviousServiceClock ?? string.Empty;

    public string GetNextCurrentServiceClock()
    {
        lock (this)
        {
            var clock = Clock.Value!;

            return $"{clock.PreviousServiceClock}.{clock.CurrentClock++}";
        }
    }
}