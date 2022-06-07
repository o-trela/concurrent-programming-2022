using System.Diagnostics;
using System.Text;

namespace BallSimulator.Data.Logging;
 
internal class LogFileWriter : ILogWriter
{
    private readonly string _logFilePath;

    public LogFileWriter(string fileName = "")
    {
        Global.EnsureDirectoryIsValid();

        if (String.IsNullOrWhiteSpace(fileName)) fileName = $"collisions({DateTime.Now:'D'yyyy-MM-dd'T'HH-mm-ss}).log";
        
        _logFilePath = Path.Combine(Global.BaseDataDirPath, fileName);
    }

    public void Write(IEnumerable<LogEntry> logEntries)
    {
        var sb = new StringBuilder();
        foreach (var logEntry in logEntries)
        {
            sb.Append('[')
                .Append(logEntry.TimeStamp)
                .Append("] : ")
                .Append(logEntry.Level)
                .Append(" @ ")
                .Append(logEntry.LineNumber)
                .Append("  \t")
                .Append(logEntry.Message)
                .AppendLine();
        }

        try
        {
            File.AppendAllText(_logFilePath, sb.ToString());
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            throw;
        }
    }

    public void Dispose()
    {
        Global.DeleteDirectory();
    }
}
