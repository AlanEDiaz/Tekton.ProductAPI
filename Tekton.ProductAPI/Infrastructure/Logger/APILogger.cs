using Serilog;

namespace Tekton.ProductAPI.Infrastructure.Logger
{
    public class APILogger : IAPILogger
    {
        public void LogDebug(string Message) => Log.Debug("{AMessage}", Message);

        public void LogInfo(string Message) => Log.Information("{AMessage}", Message);

        public void LogWarn(string Message) => Log.Warning("{AMessage}", Message);

        public void LogError(string Message) => Log.Error("{AMessage}", Message);

        public void LogFatal(string Message) => Log.Fatal("{AMessage}", Message);
    }
}
