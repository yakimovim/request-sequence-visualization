// See https://aka.ms/new-console-template for more information

using EventsReader;
using Seq.Api;

var connection = new SeqConnection("http://localhost:9090");

var result = connection.Events.EnumerateAsync(
    filter: "CorrelationId = '8c2c8da18bca48e584404190d0c5d394'",
    render: true,
    count: int.MaxValue);

var logs = new ServicesRequestLogs();

await foreach (var evt in result)
{
    logs.Add(evt);
}

logs.PrintSequenceDiagram();