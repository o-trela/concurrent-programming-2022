namespace Example;

public class Euclidean
{
    public static int GCD(int a, int b)
    {
        if (a.Equals(0))
        {
            return b;
        }
        return GCD(b % a, a);
    }
}