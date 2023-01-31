using System;
using System.Collections.Generic;

public struct MapCellCoord : IEqualityComparer<MapCellCoord>
{
    public int Row { get; }
    public int Col { get; }

    public MapCellCoord(int col, int row)
    {
        var check = (row + col) % 2;
        if (check != 0)
        {
            throw new Exception("Invalid coordinated : Row + Col should always give even value");
        }
        Row = row;
        Col = col;
    }

    public bool Equals(MapCellCoord first, MapCellCoord second)
    {
        return first.Col == second.Col && first.Row == second.Row;
    }

    public int GetHashCode(MapCellCoord obj)
    {
        return obj.GetHashCode();
    }

    public override int GetHashCode()
    {
        return $"{Col};{Row}".GetHashCode();
    }

    public MapCellCoord Move(int xMove, int zMove)
    {
        return new MapCellCoord(Col + xMove, Row + zMove);
    }
}
