using Minesweeper.Enums;
using Minesweeper.Extensions;
using Minesweeper.Interfaces;

namespace Minesweeper.Games.Minewalker;

public class MinewalkerGame(IConsoleInput console) : ITurnBasedGame
{
    private static readonly Random Rng = new();

    internal MinewalkerCell[,] Grid { get; private set; } = { };

    internal (int x, int y) CurrentPosition = (0, 0);

    internal int Lives = 2;

    //Really this should use the IOptions pattern or similar to allow for setting game parameters based on either user input or config (e.g. appsettings.json), but I ran out of time.
    public ICell[,] Build()
    {
        const int width = 8;
        const int height = 8;
        const int numberOfMines = 10;
        Lives = 2;
        State = GameState.InProgress;

        Grid = new MinewalkerCell[width, height];

        for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Grid[x, y] = new();

        Grid[0, Grid.GetUpperBound(1)].HasPlayer = true;
        CurrentPosition = (0, Grid.GetUpperBound(1));

        Grid[Grid.GetUpperBound(0), 0].Finish = true;
        
        DistributeMines(numberOfMines);

        return Grid.ToInterfaceArray();
    }

    private void DistributeMines(int numberOfMines)
    {
        while (numberOfMines > 0)
        {
            var cell = Grid[Rng.Next(Grid.GetLength(0)), Rng.Next(Grid.GetLength(1))];

            if (cell.Mined || cell.HasPlayer || cell.Finish)
                continue;

            cell.Mine();
            numberOfMines--;
        }
    }

    public ICell[,] Play()
    {
        var move = ValidationExtensions.PromptConsoleKeyPressUntilValid($"Move with your arrow keys. {Lives} lives left.", ValidateMove, console);

        MovePlayerTo(move!.Value.x, move.Value.y);

        if (Grid[CurrentPosition.x, CurrentPosition.y].HasPlayer && Grid[CurrentPosition.x, CurrentPosition.y].Finish)
        {
            RevealAllMines();
            State = GameState.Won;
            return Grid.ToInterfaceArray();
        }

        if (Lives >= 0) 
            return Grid.ToInterfaceArray();

        RevealAllMines();
        State = GameState.Lost;

        return Grid.ToInterfaceArray();
    }

    internal (int x, int y)? ValidateMove(ConsoleKey key) =>
        key switch
        {
            ConsoleKey.LeftArrow => CurrentPosition.x > 0 ? (CurrentPosition.x - 1, CurrentPosition.y) : null,
            ConsoleKey.UpArrow => CurrentPosition.y > 0 ? (CurrentPosition.x, CurrentPosition.y - 1) : null,
            ConsoleKey.RightArrow => CurrentPosition.x < Grid.GetUpperBound(0) ? (CurrentPosition.x + 1, CurrentPosition.y) : null,
            ConsoleKey.DownArrow => CurrentPosition.y < Grid.GetUpperBound(1) ? (CurrentPosition.x, CurrentPosition.y + 1) : null,
            _ => null
        };

    internal void MovePlayerTo(int x, int y)
    {
        var oldCell = Grid[CurrentPosition.x, CurrentPosition.y];
        oldCell.HasPlayer = false;

        CurrentPosition = (x, y);
        var newCell = Grid[x, y];
        newCell.HasPlayer = true;

        newCell.Step();

        if (newCell is { Mined: false, Exploded: true })
        {
            Lives--;
        }
    }

    internal GameState State { get; set; } = GameState.InProgress;

    public GameState GetState() => State;

    internal void RevealAllMines()
    {
        foreach (var cell in Grid.ToArray().Where(cell => cell.Mined))
            cell.Step(true);
    }

    public string GameOverMessage => State == GameState.Won
        ? "Congratulations! you made it out alive!"
        : "Commiserations. You were blown to smithereens.";
}