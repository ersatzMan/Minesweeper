namespace Minesweeper;

public record Move(char Key, string Name, Action<int, int> Callback)
{
    public string Print() => Key + " = " + Name;
};