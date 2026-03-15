using Minesweeper.Interfaces;

namespace Minesweeper.Games.Minewalker;

public class MinewalkerCell : ICell
{
    internal bool HasPlayer { get; set; } = false;
    internal bool Mined { get; set; } = false;
    internal bool Revealed { get; set; } = false;
    internal bool Exploded { get; set; } = false;
    internal bool Finish { get; set; } = false;

    public char Draw() =>
        HasPlayer
            ? 'O'
            : Finish 
                ? 'F'
                : Exploded
                    ? 'X'
                    : Mined 
                        ? Revealed 
                            ? '*'
                            : ' '
                        : ' ';

    internal void Step(bool safe = false)
    {
        if (Revealed)
            return;

        if (Mined && !safe)
        {
            Exploded = true;
            Mined = false;
        }

        Revealed = true;
    }

    internal void Mine() => Mined = true;
}