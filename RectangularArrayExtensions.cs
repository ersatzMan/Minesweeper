namespace Minesweeper;

public static class RectangularArrayExtensions
{
    extension<T>(T[,] arr)
    {
        public T? ElementAtOrDefault(int row, int col) =>
            row >= 0 && row < arr.GetLength(0) &&
            col >= 0 && col < arr.GetLength(1)
                ? arr[row, col]
                : default;

        public T[] ToArray() => arr.Cast<T>().ToArray();
    }
}