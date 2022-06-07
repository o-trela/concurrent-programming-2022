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

    public void Write(LogEntry[] logEntries)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (LogEntry logEntry in logEntries)
        {
            stringBuilder.Append("[");
            stringBuilder.Append(logEntry.TimeStamp);
            stringBuilder.Append("] : ");
            stringBuilder.Append(logEntry.Level.ToString());
            stringBuilder.Append(" @ ");
            stringBuilder.Append(logEntry.LineNumber);
            stringBuilder.Append("  \t");
            stringBuilder.Append(logEntry.Message);
            stringBuilder.AppendLine();
        }

        try
        {
            File.AppendAllText(_logFilePath, stringBuilder.ToString());
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            throw;
        }
    }
}
