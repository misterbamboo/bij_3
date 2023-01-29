using System;
using Unity.VisualScripting;
using UnityEngine;

public class MapDrawer : MonoBehaviour
{
    [SerializeField] GameObject[] fieldPrefabs;
    [SerializeField] GameObject[] fencePrefabs;
    [SerializeField] GameObject[] dirtPrefabs;
    [SerializeField] GameObject fenceLinkPrefab;
    [SerializeField] GameObject barnPrefab;

    [SerializeField] float hexSize = 1;
    private float hexWidth;
    private float hexHeight;

    private Map Map { get; set; }

    public void Refresh(Map map)
    {
        Map = map;
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

                DrawHexPrefab(xCoord, zCoord, x, type);

                if (type == MapCellTypes.Fence)
                {
                    DrawFenceLinks(xCoord, zCoord, x);
                }
            }
        }
    }

    private void DrawHexPrefab(int xCoord, int zCoord, int x, MapCellTypes type)
    {
        var xOff = (zCoord % 2) * (hexWidth / 2f);
        var pos = new Vector3(x * hexWidth + xOff, 0, zCoord * 3f / 4f * hexHeight);

        GameObject prefab;
        switch (type)
        {
            case MapCellTypes.Empty:
                prefab = GetRandomPrefab(fieldPrefabs);
                break;
            case MapCellTypes.Fence:
                prefab = GetRandomPrefab(fencePrefabs);
                break;
            case MapCellTypes.Field:
                prefab = GetRandomPrefab(fieldPrefabs);
                break;
            case MapCellTypes.Dirt:
                prefab = GetRandomPrefab(dirtPrefabs);
                break;
            case MapCellTypes.Barn:
                prefab = barnPrefab;
                pos += new Vector3(0, 0.5f, 0);
                break;
            default:
                prefab = null;
                break;
        }

        if (prefab == null) return;

        var hex = Instantiate(prefab, pos, Quaternion.identity);
        var hexCellScript = hex.AddComponent<HexCellScript>();
        hexCellScript.SetData(xCoord, zCoord, pos);
    }

    private void DrawFenceLinks(int xCoord, int zCoord, int x)
    {
        var xOff = (zCoord % 2) * (hexWidth / 2f);
        var pos = new Vector3(x * hexWidth + xOff, 0, zCoord * 3f / 4f * hexHeight);

        var topLeftType = Map.GetMapCellType(xCoord - 1, zCoord - 1);
        if (topLeftType == MapCellTypes.Fence)
        {
            Instantiate(fenceLinkPrefab, pos, Quaternion.Euler(0, 210, 0));
        }

        var topRightType = Map.GetMapCellType(xCoord + 1, zCoord - 1);
        if (topRightType == MapCellTypes.Fence)
        {
            Instantiate(fenceLinkPrefab, pos, Quaternion.Euler(0, -210, 0));
        }

        var leftType = Map.GetMapCellType(xCoord - 2, zCoord);
        if (leftType == MapCellTypes.Fence)
        {
            Instantiate(fenceLinkPrefab, pos, Quaternion.Euler(0, 270, 0));
        }
    }

    private GameObject GetRandomPrefab(GameObject[] prefabs)
    {
        var index = UnityEngine.Random.Range(0, prefabs.Length);
        return prefabs[index];
    }
}
