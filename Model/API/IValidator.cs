namespace BallSimulator.Presentation.Model.API;

public interface IValidator<T>
{
    bool IsValid(T value);
    bool IsNotValid(T value);
}
