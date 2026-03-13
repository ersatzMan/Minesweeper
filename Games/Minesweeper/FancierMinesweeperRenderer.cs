using MinesweeperChallenge.Interfaces;

namespace MinesweeperChallenge.Games.Minesweeper;

public class FancierMinesweeperRenderer : ITurnBasedRenderer
{
    public void Render(ICell[,] grid)
    {
        Console.Clear();

        Console.WriteLine(" =====================================");
        Console.WriteLine("||   / /  / /| / /-- -----/  / /  /-- ||");
        Console.WriteLine("||  /|/| / / |/ /--   /  /  /|/| /--  ||");
        Console.WriteLine("|| / | |/ /  | /--   /  /  / | |/__   ||");
        Console.WriteLine(" =====================================");

        var xOffset = grid.GetLength(1).ToString().Length + 1;

        Console.WriteLine();

        for (var y = 0; y < grid.GetLength(1); y++)
        {
            RenderHorizontalBorder(grid, xOffset);

            Console.Write(y + 1 + " ");

            RenderMiddle(grid, y);
        }

        RenderHorizontalBorder(grid, xOffset);

        RenderXAxis(grid, xOffset);

        Console.Write("\n");
    }

    private static void RenderHorizontalBorder(ICell[,] grid, int xOffset)
    {
        IndentLeft(xOffset);

        for (var x = 0; x < grid.GetLength(0); x++)
            Console.Write("+-");

        Console.Write("+\n");
    }

    private static void IndentLeft(int xOffset)
    {
        for (var x = 0; x < xOffset; x++)
            Console.Write(" ");
    }

    private static void RenderXAxis(ICell[,] grid, int xOffset)
    {
        IndentLeft(xOffset);

        for (var x = 0; x < grid.GetLength(0); x++)
            Console.Write(" " + (x + 1));

        Console.Write('\n');
    }

    private static void RenderMiddle(ICell[,] grid, int y)
    {
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            Console.Write("|");
            Console.Write(grid[x, y].Draw());
        }

        Console.Write("|\n");
    }
}