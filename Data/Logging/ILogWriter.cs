namespace BallSimulator.Data.Logging;

public interface ILogWriter : IDisposable
{
    Task WriteAsync(IEnumerable<LogEntry> logEntries);
    Task WriteAsync(LogEntry logEntry);
}
