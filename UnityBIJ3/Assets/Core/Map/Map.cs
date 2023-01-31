using PathFinding;
using System.Collections.Generic;

public class Map
{
    public int MapSize => mapSize;

    private int mapSize;
    private Dictionary<MapCellCoord, MapCell> mapCells;

    private AStarPathFinding AStar { get; }

    public Map(int mapSize)
    {
        this.mapSize = mapSize;
        mapCells = new Dictionary<MapCellCoord, MapCell>();
        AStar = new AStarPathFinding(new MapAStarGridInfo(this));
    }

    public void SetMapCellType(MapCellCoord coord, MapCellTypes type)
    {
        if (!IsValidCoord(coord)) return;

        var cell = GetCellAt(coord);
        cell.Type = type;
        mapCells[coord] = cell;
    }

    public MapCellTypes GetMapCellType(int xCoord, int zCoord)
    {
        var coord = new MapCellCoord(xCoord, zCoord);
        return GetMapCellType(coord);
    }

    public MapCellTypes GetMapCellType(MapCellCoord coord)
    {
        if (!IsValidCoord(coord)) return MapCellTypes.None;

        var mapCell = GetCellAt(coord);
        return mapCell.Type;
    }

    private MapCell GetCellAt(MapCellCoord coord)
    {
        if (!mapCells.ContainsKey(coord))
        {
            mapCells[coord] = new MapCell(coord);
        }
        return mapCells[coord];
    }

    private bool IsValidCoord(MapCellCoord coord)
    {
        if (coord.Col < 0 || coord.Row < 0)
        {
            return false;
        }

        // Row is offset of 1 for each (odd row number)
        if (coord.Col > mapSize * 2 || coord.Row > mapSize * 2 + 1)
        {
            return false;
        }

        return true;
    }

    public bool IsBlocked(MapCellCoord coord)
    {
        var cellType = GetMapCellType(coord);
        switch (cellType)
        {
            case MapCellTypes.None:
            case MapCellTypes.Fence:
            case MapCellTypes.Barn:
            case MapCellTypes.Turret:
            default:
                return true;
            case MapCellTypes.Empty:
            case MapCellTypes.Field:
            case MapCellTypes.Dirt:
                return false;
        }
    }
}
