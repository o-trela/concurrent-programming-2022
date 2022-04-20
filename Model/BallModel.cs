using BallSimulator.Logic;

namespace BallSimulator.Presentation.Model
{
    public class BallModel
    {
        private readonly Ball _ball;

        public int Radius => _ball.Radius;
        public Vector2 Position => _ball.Position;
        public float Speed => _ball.Speed;
        public float Direction => _ball.Direction;

        public BallModel(Ball ball)
        {
            _ball = ball;
        }
    }
}
