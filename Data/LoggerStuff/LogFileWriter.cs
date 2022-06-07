using System.Diagnostics;
using System.Text;

namespace BallSimulator.Data.LoggerStuff;
 
internal class LogFileWriter : ILogWriter
{
    private readonly string _logFilePath;

    public LogFileWriter(string fileName)
    {
        Global.EnsureDirectoryIsValid();

        _logFilePath = Path.Combine(Global.BaseDataDirPath, fileName);
    }

    public void Write(IEnumerable<LogEntry> logEntries)
    {
        var sb = new StringBuilder();
        foreach (LogEntry logEntry in logEntries)
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
}
