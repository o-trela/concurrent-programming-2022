using System.Diagnostics;
using System.Text;

namespace BallSimulator.Data.Logging;

internal class LogFileWriter : ILogWriter
{
    private readonly StreamWriter _logSw;
    private readonly StringBuilder _logBuilder = new(2048);

    public LogFileWriter(string fileName = "")
    {
        Global.EnsureDirectoryIsValid();

        if (String.IsNullOrWhiteSpace(fileName)) fileName = $"collisions({DateTime.Now:'D'yyyy-MM-dd'T'HH-mm-ss}).log";

        string logFilePath = Path.Combine(Global.BaseDataDirPath, fileName);
        _logSw = File.CreateText(logFilePath);
    }

    public async Task WriteAsync(IEnumerable<LogEntry> logEntries)
    {
        _logBuilder.Clear();

        foreach (var logEntry in logEntries)
        {
            CreateAndAppendLog(logEntry);
        }

        await Write();
    }

    public async Task WriteAsync(LogEntry logEntry)
    {
        _logBuilder.Clear();

        CreateAndAppendLog(logEntry);

        await Write();
    }

    private void CreateAndAppendLog(LogEntry entry)
    {
        _logBuilder.Append('[')
                .Append(entry.TimeStamp)
                .Append("] : ")
                .Append(entry.Level)
                .Append(" @ ")
                .Append(entry.LineNumber)
                .Append("  \t")
                .Append(entry.Message)
                .AppendLine();
    }

    private async Task Write()
    {
        try
        {
            await _logSw.WriteAsync(_logBuilder);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            throw;
        }
    }

    public void Dispose()
    {
        _logSw.Dispose();
        Global.DeleteDirectory();
    }
}
