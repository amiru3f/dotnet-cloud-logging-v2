using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Elasticsearch;

namespace Shared.Logging;

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
