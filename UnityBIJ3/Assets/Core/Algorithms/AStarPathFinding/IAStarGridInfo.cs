namespace PathFinding
{
    public interface IAStarGridInfo
    {
        int Width { get; }
        int Height { get; }

        bool IsWalkable(int x, int y);
    }
}
