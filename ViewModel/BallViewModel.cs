using BallSimulator.Logic;
using BallSimulator.Presentation.Model;

namespace BallSimulator.Presentation.ViewModel
{
    public class BallViewModel : ViewModelBase
    {
        public readonly BallModel _ball;

        public int Radius => _ball.Radius;
        public Vector2 Position => _ball.Position;
        public Vector2 Speed => _ball.Speed;

        public BallViewModel(BallModel ball)
        {
            _ball = ball;
        }
    }
}
