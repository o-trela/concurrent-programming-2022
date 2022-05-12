using System.Diagnostics;

namespace BallSimulator.Logic;

public class Timey : IDisposable
{
    private readonly Stopwatch watch;
    private readonly Action<float> _action;
    private readonly int _minTimeout;

    private bool _running = false;

    public Timey(Action<float> action)
        : this (action, 1)
    { }

    public Timey(Action<float> action, int minTimeout)
    {
        _action = action;
        _minTimeout = minTimeout;
        watch = new Stopwatch();
    }

    public void Start()
    {
        watch.Restart();
        _running = true;
        Task.Run(Movement);
    }

    public void Stop()
    {
        _running = false;
        watch.Stop();
    }

    public void Movement()
    {
        while (_running)
        {
            double delta = watch.Elapsed.TotalMilliseconds;
            watch.Restart();
            
            float scaler = (float)(delta * 0.01);
            _action(scaler.Clamp(0f, 1f));
            Thread.Sleep(_minTimeout);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Stop();
    }
}