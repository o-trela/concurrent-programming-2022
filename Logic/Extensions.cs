namespace BallSimulator.Logic
{
    public static class Extension
    { 
        public static bool Between(this float f, float a, float b)
        {
            return (f > a) && (f < b);
        }

        public static bool Between(this int f, int a, int b)
        {
            return (f > a) && (f < b);
        }
    }
}
