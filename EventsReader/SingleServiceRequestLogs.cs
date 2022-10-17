using Seq.Api.Model.Events;

namespace EventsReader;

public class SingleServiceRequestLogs
{
    public string ServiceName { get; set; }

    public string ServiceAlias { get; set; }

    public string Clock { get; set; }

    public List<EventEntity> LogEntities { get; } = new List<EventEntity>();

    public void Add(EventEntity evt)
    {
        LogEntities.Add(evt);
    }
}