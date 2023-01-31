using UnityEngine;

public class MapGizmos : MonoBehaviour
{
    [SerializeField] float hexSize = 1;
    private float hexWidth;
    private float hexHeight;

    private Map Map { get; set; }

    public void Refresh(Map map)
    {
        Map = map;
    }

    private void OnDrawGizmos()
    {
        Draw();
    }

    private void OnDrawGizmosSelected()
    {
        Draw();
    }

    private void Draw()
    {
        if (Map == null) return;

        hexWidth = Mathf.Sqrt(3) * hexSize;
        hexHeight = hexSize * 2;

        for (int x = 0; x < Map.MapSize; x++)
        {
            for (int zCoord = 0; zCoord < Map.MapSize; zCoord++)
            {
                var xCoord = (x * 2) + (zCoord % 2);
                var type = Map.GetMapCellType(xCoord, zCoord);

                Color color = Color.yellow;
                switch (type)
                {
                    case MapCellTypes.Fence:
                        color = Color.blue;
                        break;
                    case MapCellTypes.Field:
                        color = Color.green;
                        break;
                    case MapCellTypes.Barn:
                        color = Color.red;
                        break;
                    case MapCellTypes.Dirt:
                        color = Color.gray;
                        break;
                }
                DrawHex(zCoord, x, color);
            }
        }
    }

    private void DrawHex(int z, int x, Color color)
    {
        var xCorner = hexWidth / 2f;
        var zCorner = hexHeight / 4f;
        var xOff = (z % 2) * (hexWidth / 2f);
        var pos = new Vector3(x * hexWidth + xOff, 0, z * 3f / 4f * hexHeight);

        var top = pos + new Vector3(0, 0, hexSize);
        var topRight = pos + new Vector3(xCorner, 0, zCorner);
        var bottomRight = pos + new Vector3(xCorner, 0, -zCorner);
        var bottom = pos + new Vector3(0, 0, -hexSize);
        var bottomLeft = pos + new Vector3(-xCorner, 0, -zCorner);
        var topLeft = pos + new Vector3(-xCorner, 0, zCorner);

        Gizmos.color = color;
        Gizmos.DrawLine(top, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottom);
        Gizmos.DrawLine(bottom, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, top);
    }
}
