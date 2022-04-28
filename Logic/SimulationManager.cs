namespace BallSimulator.Logic
{
    public class SimulationManager
    {
        private const float MaxSpeed = 30;

        private readonly Board _board;
        private readonly int _ballDiameter;
        private readonly int _ballRadius;
        private readonly Random _rand;

        public IList<Ball> Balls { get; private set; }

        public SimulationManager(Board board, int ballDiameter)
        {
            _board = board;
            _ballDiameter = ballDiameter;
            _rand = new Random();
            _ballRadius = ballDiameter / 2;
            Balls = new List<Ball>();
        }

        public void PushBalls(float strength = 0.1f)
        {
            foreach (var ball in Balls)
            {
                ball.Move(_board.XBoundry, _board.YBoundry, strength);
            }
        }

        public IList<Ball> RandomBallCreation(int count)
        {
            Balls = new List<Ball>(count);

            for (var i = 0; i < count; i++)
            {
                Vector2 position = GetRandomPos();
                Vector2 speed = GetRandomSpeed();
                Balls.Add(new Ball(_ballDiameter, position, speed));
            }

            return Balls;
        }

        private Vector2 GetRandomPos()
        {
            int x = _rand.Next(_ballRadius, _board.Width - _ballRadius);
            int y = _rand.Next(_ballRadius, _board.Height - _ballRadius);
            return new Vector2(x, y);
        }

        private Vector2 GetRandomSpeed()
        {
            const float half = MaxSpeed / 2f;
            double x = _rand.NextDouble() * MaxSpeed - half;
            double y = _rand.NextDouble() * MaxSpeed - half;
            return new Vector2((float)x, (float)y);
        }
    }
}
