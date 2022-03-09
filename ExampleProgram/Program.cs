using System;

namespace Example
{
    class ExampleClass
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Euclidean.GCD(10, 15));
            Console.WriteLine(Euclidean.GCD(35, 10));
            Console.WriteLine(Euclidean.GCD(31, 2));

            Console.ReadKey();
        }
    }
}

