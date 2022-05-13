using System.Diagnostics;
using System.Globalization;

namespace BallSimulator.Logic;

public class FairQueuer
{
    public double AverageDelta => _executionTimer.Elapsed.TotalMilliseconds / _frames;
    public double AverageFps => _frames / _executionTimer.Elapsed.TotalSeconds;
    public ulong FrameCount => _frames;

    private readonly object listLock = new();
    private readonly object validatorLock = new();
    private readonly object localLock = new();

    private readonly Stopwatch _frameTimer;
    private readonly Stopwatch _executionTimer;
    private readonly IList<Action> _workersQueue;
    private readonly int _capacity;
    private readonly NumberFormatInfo numberFormat = NumberFormatInfo.InvariantInfo;

    private Action? _validator;
    private double _delta = 0.0;
    private ulong _frames = 0ul;
    private int _count = 0;
    private int _iterator = -1;
    private bool _loop = false;

    public FairQueuer()
        : this(16)
    { }

    public FairQueuer(int capacity)
    {
        _capacity = capacity;
        _workersQueue = new List<Action>(_capacity);
        _frameTimer = new Stopwatch();
        _executionTimer = new Stopwatch();
    }

    public void Start()
    {
        lock (localLock)
        {
            _loop = true;
            _frameTimer.Restart();
        }
        _executionTimer.Restart();
        Task.Run(Loop);
    }

    public void Pause()
    {
        lock (localLock)
        {
            _loop = false;
            _frameTimer.Stop();
        }
        _executionTimer.Stop();
    }

    public void Stop()
    {
        Pause();
        Clear();
        RemoveValidator();
    }

    private async void Loop()
    {
        IList<Task> tasks = new List<Task>(_capacity);
        Task? validatorTask = null;
        int iterator = 0;
        int count = 0;
        while (_loop)
        {
            while (Next())
            {
                lock (localLock)
                {
                    count = _count;
                    if (count >= 1) iterator = _iterator;
                }

                if (count < 1) await Task.Delay(10);

                var action = Get(iterator);
                if (action is not null)
                {
                    Task task = Task.Run(action);
                    tasks.Add(task);
                }
            }

            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
                tasks.Clear();
            }

            lock (validatorLock)
            {
                if (_validator is not null)
                {
                    validatorTask = Task.Run(_validator);
                }
            }

            if (validatorTask is not null)
            {
                await validatorTask;
            }
        }
    }

    public IDisposable Add<T>(Action<T> action) where T : struct, IConvertible
    {
        TypeCode typeCode = Type.GetTypeCode(typeof(T));

        return Add(ActionNoArgs);

        void ActionNoArgs()
        {
            T value = (T)Convert.ChangeType(_delta, typeCode, numberFormat);
            action(value);
        }
    }

    public IDisposable Add(Action action)
    {
        lock (listLock)
        {
            lock (localLock)
            {
                _count++;
            }

            _workersQueue.Add(action);
            return new Disposer(this, action);
        }
    }

    public void SetValidator(Action validator)
    {
        lock (validatorLock)
        {
            _validator = validator;
        }
    }

    public void RemoveValidator()
    {
        lock (validatorLock)
        {
            _validator = null;
        }
    }

    private void Remove(Action task)
    {
        lock (listLock)
        {
            int index = _workersQueue.IndexOf(task);
            if (index < 0) return;

            lock (localLock)
            {
                if (index < _iterator) _iterator--;
                _count--;
            }

            _workersQueue.RemoveAt(index);
        }
    }

    private bool Next()
    {
        lock (localLock)
        {
            if (_iterator < _count - 1)
            {
                _iterator++;
            }
            else
            {
                _iterator = -1;
                _delta = _frameTimer.Elapsed.TotalMilliseconds;
                _frameTimer.Restart();
                _frames++;
            }

            return _iterator >= 0;
        }
    }

    private Action? Get(int index)
    {
        lock (listLock)
        {
            lock (localLock)
            {
                if (index < 0 || index >= _count) return null;
            }

            return _workersQueue[index];
        }
    }

    private void Clear()
    {
        lock (listLock)
        {
            lock (localLock)
            {
                _count = 0;
            }
            _workersQueue.Clear();
        }
    }

    private class Disposer : IDisposable
    {
        private readonly Action _action;
        private readonly FairQueuer _fairQueuer;

        public Disposer(FairQueuer actionQueue, Action task)
        {
            _action = task;
            _fairQueuer = actionQueue;
        }

        public void Dispose()
        {
            _fairQueuer.Remove(_action);
        }
    }
}
