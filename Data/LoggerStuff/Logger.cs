using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BallSimulator.Data.LoggerStuff;

public class Logger
{
    private readonly object writeLock = new();

    private readonly ILogWriter _logWriter;
    private readonly ConcurrentQueue<LogEntry> _logs = new();
    private readonly List<LogEntry> _logEntries = new();
    
    private Task? writingAction;
    private bool _logging;

    public Logger(string fileName, bool logging = true)
        : this(new LogFileWriter(fileName), logging)
    { }
    
    public Logger(ILogWriter logWriter, bool logging = true)
    {
        _logWriter = logWriter;
        _logging = logging;
    }

    public void Start()
    {
        if (!_logging) return;

        writingAction = Task.Run(WriteLoop);
    }

    public void Stop()
    {
        _logging = false;

        writingAction?.Wait();
        WriteLog();
    }

    public void Record(LogLevel level, string message, [CallerLineNumber] int lineNumber = 0)
    {
        if (_logging) return;

        _logs.Enqueue(new LogEntry(level, message, lineNumber));
    }

    private async void WriteLoop()
    {
        while(_logging)
        {
            try
            {
                await Task.Delay(10);
                WriteLog();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }
    }

    private void WriteLog()
    {
        if (!_logs.TryPeek(out _)) return;

        lock (writeLock)
        {
            _logEntries.Clear();
            _logEntries.AddRange(_logs);
            _logWriter.Write(_logEntries);
            _logs.Clear();
        }
    }
}

