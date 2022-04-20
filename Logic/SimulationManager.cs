using System;

namespace BallSimulator.Logic
{
    public class BallManager
    {
        private readonly int _boardHeight;
        private readonly int _boardWidth;
        private readonly int _ballRadius;
        private readonly Random _rand;

        private Ball[] balls;

        public BallManager(int boardHeight, int boardWidth, int ballRadius)
        {
            _boardHeight = boardHeight;
            _boardWidth = boardWidth;
            _ballRadius = ballRadius;
            _rand = new Random();
        }

        public Ball[] RandomBallCreation(int count)
        {
            balls = new Ball[count];

            for (var i = 0; i < count; i++)
            {
                var (x, y) = GetRandomPos();
                balls[i] = new Ball(_ballRadius, x, y);
            }

            return balls;
        }

        private (int x, int y) GetRandomPos()
        {
            int x = _rand.Next(0 + _ballRadius, _boardWidth - _ballRadius);
            int y = _rand.Next(0 + _ballRadius, _boardHeight - _ballRadius);

            return (x, y);
        }
    }
}
