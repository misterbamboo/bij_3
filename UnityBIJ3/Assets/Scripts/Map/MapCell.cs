public struct MapCell
{
    public MapCellCoord Coord { get; }
    public MapCellTypes Type;

    public MapCell(MapCellCoord coord)
    {
        Coord = coord;
        Type = MapCellTypes.Empty;
    }
}
