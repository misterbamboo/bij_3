using UnityEngine;

public struct SnapPosition
{
    public int XIndex;
    public int ZIndex;
    public Vector3 Position;

    public SnapPosition(int xIndex, int zIndex, Vector3 position)
    {
        XIndex = xIndex;
        ZIndex = zIndex;
        Position = position;
    }
}
