using Common;
using Seq.Api.Model.Events;

namespace EventsReader;

public class ServicesRequestLogs
{
    private readonly IDictionary<string, SingleServiceRequestLogs> _logRecords = new Dictionary<string, SingleServiceRequestLogs>();
    private readonly IDictionary<string, string> _serviceAliases = new Dictionary<string, string>();

    public void Add(EventEntity evt)
    {
        var clock = evt.GetPropertyValue(Names.RequestClockHeaderName);

        if(clock == null) return;

        var singleServiceLogs = GetSingleServiceLogs(clock, evt);

        singleServiceLogs.Add(evt);
    }

    private SingleServiceRequestLogs GetSingleServiceLogs(string clock, EventEntity evt)
    {
        if (_logRecords.ContainsKey(clock))
        {
            return _logRecords[clock];
        }

        var serviceName = evt.GetPropertyValue(Names.CurrentServiceName)!;

        var serviceAlias = GetServiceAlias(serviceName);

        var logs = new SingleServiceRequestLogs
        {
            ServiceName = serviceName,
            ServiceAlias = serviceAlias,
            Clock = clock
        };

        _logRecords.Add(clock, logs);

        return logs;
    }

    private string GetServiceAlias(string serviceName)
    {
        if(_serviceAliases.ContainsKey(serviceName))
            return _serviceAliases[serviceName];

        var serviceAlias = $"s{_serviceAliases.Count}";
        _serviceAliases[serviceName] = serviceAlias;
        return serviceAlias;
    }

    public void PrintSequenceDiagram()
    {
        Console.WriteLine();

        PrintParticipants();

        PrintServiceLogs("");
    }

    private void PrintServiceLogs(string clock)
    {
        var logs = _logRecords[clock];

        if (clock == string.Empty)
        {
            Console.WriteLine($"User->{logs.ServiceAlias}: ");
            Console.WriteLine($"activate {logs.ServiceAlias}");
        }

        foreach (var entity in logs.LogEntities.OrderBy(e => DateTime.Parse(e.Timestamp, null, System.Globalization.DateTimeStyles.RoundtripKind)))
        {
            var boundaryClock = entity.GetPropertyValue(Names.RequestBoundaryForName);

            if (boundaryClock == null)
            {
                Console.WriteLine($"note right of {logs.ServiceAlias}: {entity.RenderedMessage}");
            }
            else
            {
                if (_logRecords.TryGetValue(boundaryClock, out var anotherLogs))
                {
                    Console.WriteLine($"{logs.ServiceAlias}->{anotherLogs.ServiceAlias}: {entity.GetPropertyValue(Names.RequestURLName)}");
                    Console.WriteLine($"activate {anotherLogs.ServiceAlias}");

                    PrintServiceLogs(boundaryClock);

                    Console.WriteLine($"{anotherLogs.ServiceAlias}->{logs.ServiceAlias}: ");
                    Console.WriteLine($"deactivate {anotherLogs.ServiceAlias}");
                }
                else
                {
                    // Call to external system
                    Console.WriteLine($"{logs.ServiceAlias}->External: {entity.GetPropertyValue(Names.RequestURLName)}");
                    Console.WriteLine($"activate External");
                    Console.WriteLine($"External->{logs.ServiceAlias}: ");
                    Console.WriteLine($"deactivate External");
                }


            }
        }

        if (clock == string.Empty)
        {
            Console.WriteLine($"{logs.ServiceAlias}->User: ");
            Console.WriteLine($"deactivate {logs.ServiceAlias}");
        }
    }

    private void PrintParticipants()
    {
        Console.WriteLine("participant \"User\" as User");

        foreach (var record in _serviceAliases)
        {
            Console.WriteLine($"participant \"{record.Key}\" as {record.Value}");
        }
    }
}