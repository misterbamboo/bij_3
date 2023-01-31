using UnityEngine;

public class HexCellScript : MonoBehaviour
{
    public int X { get; private set; }

    public int Z { get; private set; }

    public Vector3 CenterPos { get; private set; }

    public void SetData(int xCoord, int zCoord, Vector3 centerPos)
    {
        X = xCoord;
        Z = zCoord;
        CenterPos = centerPos;
    }
}
