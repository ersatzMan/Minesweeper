using MinesweeperChallenge.Interfaces;

namespace MinesweeperChallenge.Games.Minewalker;

public class BoringMinewalkerRenderer : ITurnBasedRenderer
{
    public void Render(ICell[,] grid)
    {
        Console.Clear();

        Console.WriteLine("+----------------+");
        Console.WriteLine("| MIND THE MINES |");
        Console.WriteLine("+----------------+");

        RenderHorizontalBorder(grid);

        //Rows
        for (var y = 0; y < grid.GetLength(1); y++)
        {
            Console.Write("|");

            //Columns
            for (var x = 0; x < grid.GetLength(0); x++)
                Console.Write(grid[x, y].Draw());

            Console.Write("|");
            Console.Write("\n");
        }

        RenderHorizontalBorder(grid);
        Console.Write("\n");
    }

    private static void RenderHorizontalBorder(ICell[,] grid) => Console.WriteLine("+" + string.Join(string.Empty, Enumerable.Range(1, grid.GetLength(0)).Select(_ => "-")) + "+");
}