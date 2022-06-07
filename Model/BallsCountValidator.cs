using BallSimulator.Presentation.Model.API;

namespace BallSimulator.Presentation.Model;

public class BallsCountValidator : IValidator<int>
{
    private readonly int _min;
    private readonly int _max;

    public BallsCountValidator()
        : this(int.MinValue)
    { }

    public BallsCountValidator(int min)
        : this(min, int.MaxValue)
    { }

    public BallsCountValidator(int min, int max)
    {
        _min = min;
        _max = max;
    }

    public bool IsValid(int value)
    {
        return value >= _min && value <= _max;
    }

    public bool IsNotValid(int value)
    {
        return !IsValid(value);
    }
}
