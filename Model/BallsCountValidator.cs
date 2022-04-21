using System;
using BallSimulator.Logic;

namespace BallSimulator.Presentation.Model
{
    public class BallsCountValidator : IValidator<int>
    {
        private readonly int _min;
        private readonly int _max;

        public BallsCountValidator()
            : this(Int32.MinValue)
        { }

        public BallsCountValidator(int min)
            : this(min, Int32.MaxValue)
        { }

        public BallsCountValidator(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public bool IsValid(int value)
        {
            return value.Between(_min, _max);
        }
    }
}
