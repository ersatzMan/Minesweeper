namespace Minesweeper;

public sealed class Game
{
    private static readonly Random Rng = new();

    private static Cell[,] Grid { get; set; } = {};

    public GameState State = GameState.InProgress;

    private readonly Move[] _validMoves = [
        new('R', "Reveal", RevealCells),
        new('F', "Flag", Flag)
    ];

    public string PrintMovePrompt => "Choose a move: " + string.Join(", ", _validMoves.Select(move => move.Print()));

    public const string ChooseXPrompt = "Choose an X coordinate";

    public const string ChooseYPrompt = "Choose a Y coordinate";

    public string? ResultPrompt =>
        State switch
        {
            GameState.Won => "You won! Congratulations!",
            GameState.Lost => "You lost. Commiserations.",
            _ => null
        };

    public const string PlayAgainPrompt = "Play again (Y/N)?";

    public static void Build()
    {
        const int width = 9;
        const int height = 9;
        const int numberOfMines = 10;

        Grid = new Cell[width, height];

        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
            Grid[x, y] = new();

        DistributeMines(numberOfMines);
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

    public static void Draw()
    {
        Console.Clear();
        Console.Write('\n');

        Console.WriteLine(" " + string.Join(string.Empty, Enumerable.Range(1, Grid.GetLength(0))));

        //Rows
        for (var y = 0; y < Grid.GetLength(1); y++)
        {
            Console.Write(y + 1);

            //Columns
            for (var x = 0; x < Grid.GetLength(0); x++)
                Console.Write(Grid[x, y].Draw());

            Console.Write(y + 1);

            Console.Write("\n");
        }

        Console.WriteLine(" " + string.Join(string.Empty, Enumerable.Range(1, Grid.GetLength(0))));
        Console.Write("\n");
    }

    public static int? ValidateDimension(char value, bool x = true) =>
        int.TryParse(value.ToString(), out var dim) && dim > 0 && dim < (x ? Grid.GetLength(0) + 1 : Grid.GetLength(1) + 1)
            ? dim - 1
            : null;

    public Move? ValidateMove(char key) => _validMoves.SingleOrDefault(move => move.Key == key);

    public void Play(Move move, int x, int y)
    {
        move.Callback(x, y);

        //All mines are flagged = win
        if (Grid.ToArray().Where(cell => cell.Mined).All(cell => cell.Flagged))
            State = GameState.Won;

        if (!Grid.ToArray().Any(cell => cell.Exploded)) 
            return;

        //Revealed a mine = lose
        RevealAllMines();
        State = GameState.Lost;
    }

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

        if (CountNearbyMines(x, y, out var numberOfMines))
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

    private static bool CountNearbyMines(int x, int y, out int numberOfMines)
    {
        numberOfMines = ((HashSet<Cell?>)
        [
            Grid.ElementAtOrDefault(x, y + 1),      //N
            Grid.ElementAtOrDefault(x + 1, y + 1),  //NE
            Grid.ElementAtOrDefault(x + 1, y),      //E     
            Grid.ElementAtOrDefault(x + 1, y - 1),  //SE
            Grid.ElementAtOrDefault(x, y - 1),      //S
            Grid.ElementAtOrDefault(x - 1, y - 1),  //SW
            Grid.ElementAtOrDefault(x - 1, y),      //W
            Grid.ElementAtOrDefault(x - 1, y + 1),  //NW
        ]).Count(cell => cell?.Mined ?? false);

        return numberOfMines > 0;
    }

    private static void Flag(int x, int y) => Grid[x, y].Flag();

    public enum GameState
    {
        InProgress,
        Won,
        Lost
    }
}