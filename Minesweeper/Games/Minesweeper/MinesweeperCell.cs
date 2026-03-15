using Minesweeper.Interfaces;

namespace Minesweeper.Games.Minesweeper;

public sealed class MinesweeperCell : ICell
{
    internal bool Revealed { get; private set; }
    internal bool Mined { get; private set; }
    internal bool Exploded { get; private set; }
    internal bool Flagged { get; private set; }
    internal bool Checked { get; private set; }
    internal int? NearbyMines { get; private set; }

    public void SetNearbyMines(int nearbyMines) => NearbyMines = nearbyMines;

    public void Flag()
    {
        if (Revealed)
            return;

        Flagged = !Flagged;
    }

    public void Reveal(bool safe = false)
    {
        if (Revealed)
            return;

        if (Mined && !safe)
            Exploded = true;

        Revealed = true;
    }

    public char Draw() =>
        Exploded 
            ? 'X'
            : Flagged 
                ? 'P'
                : NearbyMines != null
                    ? NearbyMines.ToString()![0]
                    : Revealed
                        ? Mined
                            ? '*'
                            : ' '
                        : '?';

    public void Mine() => Mined = true;
    public void Check() => Checked = true;
}