
using BallSimulator.Logic;

namespace BallSimulator.Presentation.Model
{
    public class LogicModel
    {
        private readonly LogicAbstractApi _logic;

        public LogicModel(LogicAbstractApi logic = default)
        {
            _logic = logic ?? LogicAbstractApi.CreateApi();
        }
    }
}
