using UnityEngine;

public struct PlaceItemAnchor
{
    public int XIndex;
    public int ZIndex;
    public Vector3 Position;

    public PlaceItemAnchor(int xIndex, int zIndex, Vector3 position)
    {
        XIndex = xIndex;
        ZIndex = zIndex;
        Position = position;
    }
}
