using MinesweeperChallenge.Interfaces;

namespace MinesweeperChallenge.Extensions;

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

    public static ICell[,] ToInterfaceArray<T>(this T[,] cells) where T : ICell
    {
        var rows = cells.GetLength(0);
        var cols = cells.GetLength(1);
        var result = new ICell[rows, cols];

        for (var i = 0; i < rows; i++)
        for (var j = 0; j < cols; j++)
            result[i, j] = cells[i, j];

        return result;
    }
}
