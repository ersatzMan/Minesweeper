namespace MinesweeperChallenge.Interfaces;

public interface IConsoleInput
{
    ConsoleKeyInfo ReadKey();
    string? ReadLine();
}