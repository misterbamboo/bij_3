using System;
using System.Collections.Generic;

public class Map
{
    public int MapSize => mapSize;

    private int mapSize;
    private Dictionary<MapCellCoord, MapCell> mapCells;

    public Map(int mapSize)
    {
        this.mapSize = mapSize;
        mapCells = new Dictionary<MapCellCoord, MapCell>();
    }

    public void SetMapCellType(MapCellCoord coord, MapCellTypes type)
    {
        ValidateCoord(coord);

        var cell = GetCellAt(coord);
        cell.Type = type;
        mapCells[coord] = cell;
    }

    public MapCellTypes GetMapCellType(int xCoord, int zCoord)
    {
        var coord = new MapCellCoord(xCoord, zCoord);
        ValidateCoord(coord);

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

    private void ValidateCoord(MapCellCoord coord)
    {
        if (coord.Col < 0 || coord.Row < 0)
        {
            throw new Exception("Coordinates are out of bound");
        }

        // Row is offset of 1 for each (odd row number)
        if (coord.Col > mapSize * 2 || coord.Row > mapSize * 2 + 1)
        {
            throw new Exception("Coordinates are out of bound");
        }
    }
}
