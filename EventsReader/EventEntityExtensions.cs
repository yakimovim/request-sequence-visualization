using Seq.Api.Model.Events;

namespace EventsReader;

public static class EventEntityExtensions
{
    public static string? GetPropertyValue(this EventEntity evt, string propertyName)
    {
        return evt.Properties.FirstOrDefault(p => p.Name == propertyName)?.Value?.ToString();
    }
}