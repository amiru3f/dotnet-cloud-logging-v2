using System.Net;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Network;

namespace Shared.Logging;

public static class Extensions
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger(string applicationName) => (hostingContext, loggerConfiguration) =>
    {
        Serilog.Debugging.SelfLog.Enable(Console.Error);
        loggerConfiguration.MinimumLevel.Information();
        loggerConfiguration.Enrich.FromLogContext();
        loggerConfiguration.Enrich.WithProperty("ApplicationName", applicationName);

        loggerConfiguration.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);
        loggerConfiguration.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning);
        loggerConfiguration.MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning);
        loggerConfiguration.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning);

        loggerConfiguration
        .MinimumLevel.Verbose().WriteTo.FluentdWithTCP("127.0.0.1", 24225, "UnTagged");
    };

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