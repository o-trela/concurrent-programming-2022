namespace BallSimulator.Logic
{
    public static class Extension
    {
        public static bool Between(this float value, float min, float max, float padding = 0f)
        {
            if (padding < 0f) throw new ArgumentException("Padding must be a positive number!", nameof(padding));
            return (value - padding >= min) && (value + padding <= max);
        }

        public static bool Between(this int value, int min, int max)
        {
            return (value >= min) && (value <= max);
        }
    }
}
