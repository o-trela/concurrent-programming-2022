using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BallSimulator.Data.Logger;

public class Logger
{
    private readonly ILogWriter _logWriter;
    private readonly ConcurrentQueue<LogEntry> _logs;

    private Task writingAction;

    private bool _logging;

    public Logger(ILogWriter logWriter, bool logging = true)
    : this(logging)
    {
        _logWriter = logWriter;
        _logging = logging;
    }

    public Logger(string fileName, bool logging = true)
    : this(logging) 
    {
        _logging = logging;
        _logWriter = new LogFileWriter(fileName);
    }

    private Logger(bool logging)
    {
        _logs = new ConcurrentQueue<LogEntry>();
    }

    public void Start()
    {
        if (!_logging) return;

        writingAction = Task.Run(() => WriteLoop());
    }

    public void Stop()
    {
        _logging = false;

        writingAction.Wait();
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
        if (!_logs.TryPeek(out LogEntry temp)) return;

        List<LogEntry> logEntries = new List<LogEntry>();
        while (_logs.TryDequeue(out LogEntry entry))
        {
            logEntries.Add(entry);
        }

        _logWriter.Write(logEntries.ToArray());
    }
}

