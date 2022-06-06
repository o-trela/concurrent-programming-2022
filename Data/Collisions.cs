namespace BallSimulator.Data;

public static class Collisions
{
    private static readonly HashSet<(IBall, IBall)> collisions = new(16);
    
    public static HashSet<(IBall, IBall)> Get(IList<IBall> balls)
    {
        collisions.Clear();
        foreach (var ball1 in balls)
        {
            foreach (var ball2 in balls)
            {
                if (ball1 == ball2) continue;
                if (ball1.Touches(ball2)) collisions.Add((ball1, ball2));
            }
        }

        return collisions;
    }

    public static (Vector2 speedOne, Vector2 speedTwo) CalculateSpeeds(IBall ball1, IBall ball2)
    {
        float distance = Vector2.Distance(ball1.Position, ball2.Position);

        Vector2 normal = new((ball2.Position.X - ball1.Position.X) / distance, (ball2.Position.Y - ball1.Position.Y) / distance);
        Vector2 tangent = new(-normal.Y, normal.X);

        Vector2 ball1Speed = ball1.Speed;
        Vector2 ball2Speed = ball2.Speed;
        if (Vector2.Scalar(ball1Speed, normal) < 0f) return (ball1Speed, ball2Speed);

        float ball1Radius = ball1.Radius;
        float ball2Radius = ball2.Radius;
        float ball1Weight = ball1Radius * ball1Radius;
        float ball2Weight = ball2Radius * ball2Radius;

        float dpNorm1 = ball1Speed.X * normal.X + ball1Speed.Y * normal.Y;
        float dpNorm2 = ball2Speed.X * normal.X + ball2Speed.Y * normal.Y;

        float dpTan1 = ball1Speed.X * tangent.X + ball1Speed.Y * tangent.Y;
        float dpTan2 = ball2Speed.X * tangent.X + ball2Speed.Y * tangent.Y;

        float momentum1 = (dpNorm1 * (ball1Weight - ball2Weight) + 2.0f * ball2Weight * dpNorm2) / (ball1Weight + ball2Weight);
        float momentum2 = (dpNorm2 * (ball2Weight - ball1Weight) + 2.0f * ball1Weight * dpNorm1) / (ball2Weight + ball1Weight);

        Vector2 newVelocity1 = new(tangent.X * dpTan1 + normal.X * momentum1, tangent.Y * dpTan1 + normal.Y * momentum1);
        Vector2 newVelocity2 = new(tangent.X * dpTan2 + normal.X * momentum2, tangent.Y * dpTan2 + normal.Y * momentum2);

        return (newVelocity1, newVelocity2);
    }
}
