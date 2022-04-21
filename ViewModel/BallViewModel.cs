using BallSimulator.Logic;

namespace BallSimulator.Presentation.ViewModel
{
    public class BallViewModel : ViewModelBase
    {
        private readonly Ball _ball;

        public int Radius => _ball.Radius;
        public Vector2 Position => _ball.Position;
        public Vector2 Speed => _ball.Speed;

        public BallViewModel(Ball ball)
        {
            _ball = ball;
        }
    }
}
