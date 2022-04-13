namespace Logic
{
    public class BallManager
    {
        private int _boardHeight;
        private int _boardWidth;
        private int _ballRadius;
        private Ball[] balls;

        public BallManager(int boardHeight, int boardWidth, int ballRadius)
        {
            _boardHeight = boardHeight;
            _boardWidth = boardWidth;
            _ballRadius = ballRadius;
        }

        public Ball[] RandomBallCreation(int count)
        {
            balls = new Ball[count];
            int x, y;

            for (int i = 0; i < count; i++)
            {
                (x, y) = GetRandomPos();
                balls[i] = new(_ballRadius, x, y);
            }

            return balls;
        }

        private (int, int) GetRandomPos()
        {
            Random rand = Random.Shared;

            int x = rand.Next(0 + _ballRadius, _boardWidth - _ballRadius);
            int y = rand.Next(0 + _ballRadius, _boardHeight - _ballRadius);

            return (x, y);
        }
    }
}
