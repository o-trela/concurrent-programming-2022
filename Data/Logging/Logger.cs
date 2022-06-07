using BallSimulator.Data.API;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BallSimulator.Data.Logging;

public class Logger : ILogger, IDisposable
{
    private readonly object writeLock = new();

    private readonly ILogWriter _logWriter;
    private readonly ConcurrentQueue<LogEntry> _logQueue = new();
    private readonly List<LogEntry> _logEntries = new();

    private Task? _writingAction;
    private bool _logging;

    public Logger(string fileName = "")
        : this(new LogFileWriter(fileName))
    { }

    public Logger(ILogWriter logWriter)
    {
        _logWriter = logWriter;
        _logging = false;

        Start();
    }

    public void LogInfo(string message, [CallerLineNumber] int lineNumber = -1) => Log(message, LogLevel.Info, lineNumber);
    public void LogWarning(string message, [CallerLineNumber] int lineNumber = -1) => Log(message, LogLevel.Warning, lineNumber);
    public void LogError(string message, [CallerLineNumber] int lineNumber = -1) => Log(message, LogLevel.Error, lineNumber);

    private void Start()
    {
        if (_logging) return;

        _logging = true;
        _writingAction = Task.Run(WriteLoop);
    }

    private void Stop()
    {
        _logging = false;

        _writingAction?.Wait();
        WriteLogs();
    }

    private void Log(string message, LogLevel level, int lineNumber)
    {
        if (!_logging) return;

        _logQueue.Enqueue(new LogEntry(level, message, lineNumber));
    }

    private async void WriteLoop()
    {
        while (!_logging)
        {
            try
            {
                await Task.Delay(50);
                WriteLogs();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }
    }

    private void WriteLogs()
    {
        lock (writeLock)
        {
            if (_logQueue.IsEmpty) return;

            _logEntries.Clear();
            _logEntries.AddRange(_logQueue);
            _logQueue.Clear();
            _logWriter.Write(_logEntries);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        Stop();
        _logWriter.Dispose();
        _writingAction?.Dispose();
    }
}

