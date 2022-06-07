namespace BallSimulator.Data.LoggerStuff;

public struct LogEntry
{
    public readonly string TimeStamp;
    public readonly LogLevel Level;
    public readonly int LineNumber;
    public readonly string Message;

    public LogEntry(LogLevel level, string message, int lineNumber)
    {
        TimeStamp = DateTime.Now.ToString("dd-MM-yyyy - HH:mm:ss:fff");
        Level = level;
        LineNumber = lineNumber;
        Message = message; 
    }
}
