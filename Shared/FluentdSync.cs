using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Network;

namespace Shared.Logging;

public static class FluentdSyncExtensions
{
    public static LoggerConfiguration FluentdWithTCP(this LoggerSinkConfiguration loggerSinkConfiguration, string ip, int port, string defaultTag)
    {
        return loggerSinkConfiguration.TCPSink($"tcp://{ip}", port, new FluentdJsonFormatter(defaultTag));
    }

    public static LoggerConfiguration FluentdWithUDP(this LoggerSinkConfiguration loggerSinkConfiguration, string ip, int port, string defaultTag)
    {
        return loggerSinkConfiguration.UDPSink($"udp://{ip}", port, new FluentdJsonFormatter(defaultTag));
    }

    public static long ToEpochTime(this DateTimeOffset offset)
    {
        var utcDate = offset.ToUniversalTime();
        long baseTicks = 621355968000000000;
        long tickResolution = 10000000;
        long epoch = (utcDate.Ticks - baseTicks) / tickResolution;
        long epochTicks = (epoch * tickResolution) + baseTicks;
        var date = new DateTime(epochTicks, DateTimeKind.Utc);

        return epoch;
    }
}

public class FluentdJsonFormatter : ITextFormatter
{
    private readonly ElasticsearchJsonFormatter jsonFormatter;
    private string defaultTag;

    public FluentdJsonFormatter(string defaultTag)
    {
        jsonFormatter = new ElasticsearchJsonFormatter();
        this.defaultTag = defaultTag;
    }

    public void Format(LogEvent logEvent, TextWriter output)
    {
        TextWriter logEventWriter = new StringWriter();
        jsonFormatter.Format(logEvent, logEventWriter);
        if (logEvent.Properties.TryGetValue("SourceContext", out var sourceContext))
        {
            defaultTag = sourceContext.ToString();
        }
        else
        {
            defaultTag = $"\"{defaultTag}\"";
        }

        output.Write($"[{defaultTag}, {DateTimeOffset.Now.ToEpochTime()}, {logEventWriter}]");
    }
}
