using System;
using System.Collections.Generic;

namespace BallSimulator.Logic
{
    public class SimulationManager
    {
        private readonly Board _board;
        private readonly int _ballDiameter;
        private readonly int _ballRadiu;
        private readonly Random _rand;

        public List<Ball> Balls { get; private set; }

        public SimulationManager(Board board, int ballDiameter)
        {
            _board = board;
            _ballDiameter = ballDiameter;
            _rand = new Random();
            _ballRadiu = ballDiameter / 2;
        }

        public void PushBalls(float strength = 0.1f)
        {
            foreach (var ball in Balls)
            {
                ball.Move(_board.XBoundry, _board.YBoundry, strength);
            }
        }

        public List<Ball> RandomBallCreation(int count)
        {
            Balls = new List<Ball>(count);

            for (var i = 0; i < count; i++)
            {
                var (posX, posY) = GetRandomPos();
                var (speedX, speedY) = GetRandomSpeed();
                Balls.Add(new Ball(_ballDiameter, posX, posY, speedX, speedY));
            }

            return Balls;
        }

        private (int x, int y) GetRandomPos()
        {
            int x = _rand.Next(0 + _ballDiameter, _board.Width - _ballDiameter);
            int y = _rand.Next(0 + _ballDiameter, _board.Height - _ballDiameter);
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
