using BallSimulator.Logic;

namespace BallSimulator.Presentation.Model
{
    public class BallModel
    {
        private readonly Ball _ball;

        public int Radius => _ball.Radius;
        public Vector2 Position => _ball.Position;
        public Vector2 Speed => _ball.Speed;

        public BallModel(Ball ball)
        {
            _ball = ball;
        }
    }
}
