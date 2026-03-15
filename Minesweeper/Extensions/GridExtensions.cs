namespace Minesweeper.Extensions;

public static class GridExtensions
{
    public static int RadialCount<T>(this T[,] grid, int x, int y, Func<T, bool> predicate, bool includeCentre = false)
    {
        var result = new HashSet<T?>
        {
            grid.ElementAtOrDefault(x, y + 1),      //N
            grid.ElementAtOrDefault(x + 1, y + 1), //NE
            grid.ElementAtOrDefault(x + 1, y),      //E     
            grid.ElementAtOrDefault(x + 1, y - 1), //SE
            grid.ElementAtOrDefault(x, y - 1),      //S
            grid.ElementAtOrDefault(x - 1, y - 1), //SW
            grid.ElementAtOrDefault(x - 1, y),      //W
            grid.ElementAtOrDefault(x - 1, y + 1), //NW
        }.Where(cell => cell != null).ToHashSet();
        
        if (includeCentre)
            result.Add(grid.ElementAtOrDefault(x, y)); //centre
        
        return result!.Count(predicate!);
    }
}