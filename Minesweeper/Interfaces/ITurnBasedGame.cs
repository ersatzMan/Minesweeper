using Minesweeper.Enums;

namespace Minesweeper.Interfaces;

public interface ITurnBasedGame
{
    ICell[,] Build();
    ICell[,] Play();
    GameState GetState();
    string? GameOverMessage { get; }
}