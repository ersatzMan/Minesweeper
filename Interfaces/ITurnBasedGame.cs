using MinesweeperChallenge.Enums;

namespace MinesweeperChallenge.Interfaces;

public interface ITurnBasedGame
{
    ICell[,] Build();
    ICell[,] Play();
    GameState GetState();
    string? GameOverMessage { get; }
}