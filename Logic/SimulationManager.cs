using System;

namespace BallSimulator.Logic
{
    public class SimulationManager
    {
        private readonly Board _board;
        private readonly int _ballRadius;
        private readonly Random _rand;

        private Ball[] balls;

        public SimulationManager(Board board, int ballRadius)
        {
            _board = board;
            _ballRadius = ballRadius;
            _rand = new Random();
        }

        public void PushBalls(float strength = 0.1f)
        {
            foreach (var ball in balls)
            {
                ball.Move(strength);
            }
        }

        public Ball[] RandomBallCreation(int count)
        {
            balls = new Ball[count];

            for (var i = 0; i < count; i++)
            {
                var (posX, posY) = GetRandomPos();
                var (speedX, speedY) = GetRandomSpeed();
                balls[i] = new Ball(_ballRadius, posX, posY, speedX, speedY);
            }

            return balls;
        }

        private (int x, int y) GetRandomPos()
        {
            int x = _rand.Next(0 + _ballRadius, _board.Width - _ballRadius);
            int y = _rand.Next(0 + _ballRadius, _board.Height - _ballRadius);

            return (x, y);
        }

        private (float x, float y) GetRandomSpeed()
        {
            double x = _rand.NextDouble() * 2 - 1;
            double y = _rand.NextDouble() * 2 - 1;

            return  ((float) x, (float) y);
        }
    }
}
