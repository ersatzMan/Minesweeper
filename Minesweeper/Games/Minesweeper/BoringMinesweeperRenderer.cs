using Minesweeper.Interfaces;

namespace Minesweeper.Games.Minesweeper;

public class BoringMinesweeperRenderer : ITurnBasedRenderer
{
    public void Render(ICell[,] grid)
    {
        Console.Clear();
        Console.WriteLine("**************************************");
        Console.WriteLine("* LEGALLY DISTINCT MINESWEEPING GAME *");
        Console.WriteLine("**************************************");
        Console.WriteLine();
        Console.WriteLine(" " + string.Join(string.Empty, Enumerable.Range(1, grid.GetLength(0))));

        //Rows
        for (var y = 0; y < grid.GetLength(1); y++)
        {
            Console.Write(y + 1);

            //Columns
            for (var x = 0; x < grid.GetLength(0); x++)
                Console.Write(grid[x, y].Draw());

            Console.Write(y + 1);

            Console.Write("\n");
        }

        Console.WriteLine(" " + string.Join(string.Empty, Enumerable.Range(1, grid.GetLength(0))));
        Console.Write("\n");
    }
}