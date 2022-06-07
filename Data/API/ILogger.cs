using System.Runtime.CompilerServices;

namespace BallSimulator.Data.API;

public interface ILogger : IDisposable
{
    public void LogInfo(string message, [CallerLineNumber] int lineNumber = -1);
    public void LogWarning(string message, [CallerLineNumber] int lineNumber = -1);
    public void LogError(string message, [CallerLineNumber] int lineNumber = -1);
}