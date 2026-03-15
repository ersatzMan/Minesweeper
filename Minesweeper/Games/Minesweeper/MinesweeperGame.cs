using Minesweeper.Enums;
using Minesweeper.Extensions;
using Minesweeper.Interfaces;

namespace Minesweeper.Games.Minesweeper;

public sealed class MinesweeperGame(IConsoleInput console) : ITurnBasedGame
{
    private static readonly Random Rng = new();

    private static MinesweeperCell[,] Grid { get; set; } = {};

    private static KeyPressMove[] ValidMoves => [
        new('R', "Reveal", RevealCells),
        new('F', "Flag", Flag)
    ];

    private GameState State { get; set; } = GameState.InProgress;

    private static string PrintMovePrompt => "Choose a move: " + string.Join(", ", ValidMoves.Select(move => move.Print));

    private const string ChooseXPrompt = "Choose an X coordinate";

    private const string ChooseYPrompt = "Choose a Y coordinate";

    public GameState GetState() => State;

    public string? GameOverMessage =>
        State switch
        {
            GameState.Won => "You won! Congratulations!",
            GameState.Lost => "You lost. Commiserations.",
            _ => null
        };

    public ICell[,] Build()
    {
        const int width = 9;
        const int height = 9;
        const int numberOfMines = 10;

        Grid = new MinesweeperCell[width, height];

        for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Grid[x, y] = new();

        DistributeMines(numberOfMines);

        return Grid.ToInterfaceArray();
    }

    private static void DistributeMines(int numberOfMines)
    {
        while (numberOfMines > 0)
        {
            var cell = Grid[Rng.Next(Grid.GetLength(0)), Rng.Next(Grid.GetLength(1))];

            if (cell.Mined)
                continue;

            cell.Mine();
            numberOfMines--;
        }
    }

    public ICell[,] Play()
    {
        var move = ValidationExtensions.PromptKeyPressUntilValid(PrintMovePrompt, ValidateMove, console)!;
        var x = (int)ValidationExtensions.PromptKeyPressUntilValid(ChooseXPrompt, c => ValidateDimension(c), console)!;
        var y = (int)ValidationExtensions.PromptKeyPressUntilValid(ChooseYPrompt, c => ValidateDimension(c, false), console)!;

        move.Callback(x, y);

        //All mines are flagged = win
        if (Grid.ToArray().Where(cell => cell.Mined).All(cell => cell.Flagged))
            State = GameState.Won;

        if (!Grid.ToArray().Any(cell => cell.Exploded))
            return Grid.ToInterfaceArray();

        //Revealed a mine = lose
        RevealAllMines();
        State = GameState.Lost;

        return Grid.ToInterfaceArray();
    }

    private static int? ValidateDimension(char value, bool x = true) =>
        int.TryParse(value.ToString(), out var dim) && dim > 0 && dim < (x ? Grid.GetLength(0) + 1 : Grid.GetLength(1) + 1)
            ? dim - 1
            : null;

    private static KeyPressMove? ValidateMove(char key) => ValidMoves.SingleOrDefault(move => move.Key == key);

    /// <summary>
    /// Reveal Cells "joined" to the chosen Cell using a depth-search-first "flood fill" algorithm
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private static void RevealCells(int x, int y)
    {
        if (x < 0 || x >= Grid.GetLength(0) ||
            y < 0 || y >= Grid.GetLength(1) ||
            Grid[x, y].Revealed ||
            Grid[x, y].NearbyMines > 0 ||
            Grid[x, y].Checked)
        {
            return;
        }

        if (Grid[x, y].Mined) //game over!
        {
            Grid[x, y].Reveal();
            return;
        }

        var numberOfMines = Grid.RadialCount(x, y, cell => cell.Mined);

        if (numberOfMines > 0)
        {
            Grid[x, y].SetNearbyMines(numberOfMines);
            return;
        }

        Grid[x, y].Reveal();
        Grid[x, y].Check();

        // Recursively visit all 4 connected neighbors
        RevealCells(x + 1, y);
        RevealCells(x - 1, y);
        RevealCells(x, y + 1);
        RevealCells(x, y - 1);
    }

    private static void RevealAllMines()
    {
        foreach (var cell in Grid.ToArray().Where(cell => cell.Mined))
            cell.Reveal(true);
    }

    private static void Flag(int x, int y) => Grid[x, y].Flag();
}

public record KeyPressMove(char Key, string Name, Action<int, int> Callback)
{
    public string Print => Key + " = " + Name;
}