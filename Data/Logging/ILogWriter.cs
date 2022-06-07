namespace BallSimulator.Data.Logging;

public interface ILogWriter : IDisposable
{
    void Write(IEnumerable<LogEntry> logEntries);
}
