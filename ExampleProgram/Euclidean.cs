namespace ExampleProgram;

public class Euclidean
{
    public int GCD(int a, int b) => a == 0 ? b : GCD(b % a, a);
}