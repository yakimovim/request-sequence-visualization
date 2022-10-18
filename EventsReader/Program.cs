// See https://aka.ms/new-console-template for more information

using EventsReader;
using Seq.Api;

var connection = new SeqConnection("http://localhost:9090");

var result = connection.Events.EnumerateAsync(
    filter: "CorrelationId = '3d2a3896e2104cdf845f76724610446f'",
    render: true,
    count: int.MaxValue);

var logs = new ServicesRequestLogs();

await foreach (var evt in result)
{
    logs.Add(evt);
}

logs.PrintSequenceDiagram();