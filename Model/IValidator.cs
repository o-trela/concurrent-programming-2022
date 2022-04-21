namespace BallSimulator.Presentation.Model
{
    public interface IValidator<T>
    {
        bool IsValid(T value);
    }
}