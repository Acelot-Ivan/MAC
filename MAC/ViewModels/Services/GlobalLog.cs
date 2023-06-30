using Serilog;

namespace MAC.ViewModels.Services
{
    public static class GlobalLog
    {
        public static readonly Serilog.Core.Logger Log = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("ErrorLog.txt")
            .CreateLogger();
    }
}