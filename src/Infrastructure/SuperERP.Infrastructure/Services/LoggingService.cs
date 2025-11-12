using Serilog;

namespace SuperERP.Infrastructure.Services;

public static class LoggingService
{
    public static void ConfigureLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/supererp-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}
