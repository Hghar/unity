namespace Extensions
{
    public static class TwoDimensionalArrayExtensions
    {
        public static bool IsInside<T>(this T[,] array, int x, int y)
        {
            return x < array.GetLength(0) && x >= 0 && y < array.GetLength(1) && y >= 0;
        }
    }
}