namespace Minesweeper.Interfaces;

public interface ITurnBasedRenderer
{
    void Render(ICell[,] grid);
}