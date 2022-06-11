using BallSimulator.Data.API;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace BallSimulator.Data.Logging;

public class Logger : ILogger, IDisposable
{
    private readonly ILogWriter _logWriter;
    private readonly BlockingCollection<LogEntry> _logQueue = new();

    private Task? _writingAction;

    public Logger(string fileName = "")
        : this(new LogFileWriter(fileName))
    { }

    public Logger(ILogWriter logWriter)
    {
        _logWriter = logWriter;

        Start();
    }

    public void LogInfo(string message, [CallerLineNumber] int lineNumber = -1) => Log(message, LogLevel.Info, lineNumber);
    public void LogWarning(string message, [CallerLineNumber] int lineNumber = -1) => Log(message, LogLevel.Warning, lineNumber);
    public void LogError(string message, [CallerLineNumber] int lineNumber = -1) => Log(message, LogLevel.Error, lineNumber);

    private void Start()
    {
        _writingAction = Task.Run(WriteLoop);
    }

    private void Stop()
    {
        _logQueue.CompleteAdding();
        _writingAction?.Wait();
    }

    private void Log(string message, LogLevel level, int lineNumber)
    {
        _logQueue.Add(new LogEntry(level, message, lineNumber));
    }

    private async void WriteLoop()
    {
        var logsEnumerable = _logQueue.GetConsumingEnumerable();
        try
        {
            foreach (var logEntry in logsEnumerable)
            {
                await _logWriter.WriteAsync(logEntry);
            }
        }
        catch (ObjectDisposedException)
        { }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        Stop();
        _writingAction?.Dispose();
        _writingAction = null;
        _logWriter.Dispose();
        _logQueue.Dispose();
    }
}

