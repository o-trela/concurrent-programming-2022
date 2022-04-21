using System;

namespace BallSimulator.Logic
{
    public class SimulationManager
    {
        private readonly Board _board;
        private readonly int _ballRadius;
        private readonly Random _rand;

        public Ball[] Balls { get; private set; }

        public SimulationManager(Board board, int ballRadius)
        {
            _board = board;
            _ballRadius = ballRadius;
            _rand = new Random();
        }

        public void PushBalls(float strength = 0.1f)
        {
            foreach (var ball in Balls)
            {
                ball.Move(_board.xBoundry, _board.yBoundry, strength);
            }
        }

        public Ball[] RandomBallCreation(int count)
        {
            Balls = new Ball[count];

            for (var i = 0; i < count; i++)
            {
                var (posX, posY) = GetRandomPos();
                var (speedX, speedY) = GetRandomSpeed();
                Balls[i] = new Ball(_ballRadius, posX, posY, speedX, speedY);
            }

            return Balls;
        }

        private (int x, int y) GetRandomPos()
        {
            int x = _rand.Next(0 + _ballRadius, _board.Width - _ballRadius);
            int y = _rand.Next(0 + _ballRadius, _board.Height - _ballRadius);
            return (x, y);
        }

        private (float x, float y) GetRandomSpeed()
        {
            double x = _rand.NextDouble() * 20 - 10;
            double y = _rand.NextDouble() * 20 - 10;

            return ((float)x, (float)y);
        }
    }
}
