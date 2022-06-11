namespace BallSimulator.Data.Logging;

public record LogEntry(
    LogLevel Level,
    string Message,
    int LineNumber
    )
{
    public string TimeStamp { get; init; } = DateTime.Now.ToString("dd-MM-yyyy - HH:mm:ss:fff");
}
