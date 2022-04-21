namespace BallSimulator.Logic
{
    public static class Extension
    {
        public static bool Between(this float value, float min, float max)
        {
            return (value >= min) && (value <= max);
        }

        public static bool Between(this int value, int min, int max)
        {
            return (value >= min) && (value <= max);
        }
    }
}
