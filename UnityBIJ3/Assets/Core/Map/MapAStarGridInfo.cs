using PathFinding;

public class MapAStarGridInfo : IAStarGridInfo
{
    public int Width => Map.MapSize * 2;

    public int Height => Map.MapSize;

    private Map Map { get; }

    public MapAStarGridInfo(Map map)
    {
        Map = map;
    }

    public bool IsWalkable(int x, int y)
    {
        return Map.IsBlocked(new MapCellCoord(x * 2, y));
    }
}
