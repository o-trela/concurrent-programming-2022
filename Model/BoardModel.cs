using BallSimulator.Logic;

namespace Model
{
    internal class BoardModel
    {
        private readonly Board _board;
        public int Height => _board.Height;
        public int Width => _board.Width;

        public BoardModel(Board board)
        {
            _board = board;
        }
    }
}
