namespace BallSimulator.Data;

public static class ThreadManager
{
    private static readonly FairQueuer _fairQueuer = new(Environment.ProcessorCount * 2);

    public static double AverageDeltaTime => _fairQueuer.AverageDelta;
    public static double AverageFps => _fairQueuer.AverageFps;
    public static double FrameCount => _fairQueuer.FrameCount;

    public static void Start() => _fairQueuer.Start();
    public static void Pause() => _fairQueuer.Pause();
    public static void Stop() => _fairQueuer.Stop();
    public static IDisposable Add(Action action) => _fairQueuer.Add(action);
    public static IDisposable Add<T>(Action<T> action) where T : struct, IConvertible => _fairQueuer.Add(action);
    public static void SetValidator(Action validator) => _fairQueuer.SetValidator(validator);
    public static void RemoveValidator() => _fairQueuer.RemoveValidator();
}
