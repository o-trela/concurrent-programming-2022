namespace ExampleProgram;

public static class Euclidean
{
    public static int GCD(int a, int b)
    {
        if (a == 0 && b == 0) throw new ArgumentException("You can not pass two zeros! (bussiness logic)");
        return a == 0 ? b : GCD(b % a, a);
    }
}