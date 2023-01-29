public class HexCellHoverEvent : IGameEventBase
{
    public MapCellCoord Coord { get; }
    public bool IsBlocked { get; }

    public HexCellHoverEvent(MapCellCoord coord, bool isBlocked)
    {
        Coord = coord;
        IsBlocked = isBlocked;
    }
}
