namespace MinesweeperChallenge.Interfaces;

public class ConsoleInput : IConsoleInput
{
    public ConsoleKeyInfo ReadKey() => Console.ReadKey(true);
    public string? ReadLine() => Console.ReadLine();
}