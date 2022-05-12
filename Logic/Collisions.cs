namespace BallSimulator.Logic;

public static class Collisions
{
    public static ISet<(IBall, IBall)> Get(IList<IBall> balls)
    {
        var collisions = new HashSet<(IBall, IBall)>(balls.Count);
        foreach (var ball1 in balls)
        {
            foreach (var ball2 in balls)
            {
                if (ball1 == ball2) continue;
                if (ball1.Touches(ball2) && !collisions.Contains((ball2, ball1))) collisions.Add((ball1, ball2));
            }
        }
        return collisions;
    }

    public static (Vector2, Vector2) CalculateSpeeds(IBall ball1, IBall ball2)
    {
        float distance = Vector2.Distance(ball1.Position, ball2.Position);

        Vector2 normal = new((ball2.Position.X - ball1.Position.X) / distance, (ball2.Position.Y - ball1.Position.Y) / distance);
        Vector2 tangent = new(-normal.Y, normal.X);

        if (Vector2.Scalar(ball1.Speed, normal) < 0f) return (ball1.Speed, ball2.Speed);

        float dpTan1 = ball1.Speed.X * tangent.X + ball1.Speed.Y * tangent.Y;
        float dpTan2 = ball2.Speed.X * tangent.X + ball2.Speed.Y * tangent.Y;

        float dpNorm1 = ball1.Speed.X * normal.X + ball1.Speed.Y * normal.Y;
        float dpNorm2 = ball2.Speed.X * normal.X + ball2.Speed.Y * normal.Y;

        float momentum1 = (dpNorm1 * (ball1.Radius - ball2.Radius) + 2.0f * ball2.Radius * dpNorm2) / (ball1.Radius + ball2.Radius);
        float momentum2 = (dpNorm2 * (ball2.Radius - ball1.Radius) + 2.0f * ball1.Radius * dpNorm1) / (ball1.Radius + ball2.Radius);

        Vector2 newVelocity1 = new(tangent.X * dpTan1 + normal.X * momentum1, tangent.Y * dpTan1 + normal.Y * momentum1);
        Vector2 newVelocity2 = new(tangent.X * dpTan2 + normal.X * momentum2, tangent.Y * dpTan2 + normal.Y * momentum2);

        return (newVelocity1, newVelocity2);
    }
}
