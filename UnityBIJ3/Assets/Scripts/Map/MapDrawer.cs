using UnityEngine;

public class MapDrawer : MonoBehaviour
{
    [SerializeField] GameObject[] emptyPrefabs;
    [SerializeField] GameObject[] fieldPrefabs;
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

                DrawHexPrefab(zCoord, x, type);
            }
        }
    }

    private void DrawHexPrefab(int z, int x, MapCellTypes type)
    {
        var xOff = (z % 2) * (hexWidth / 2f);
        var pos = new Vector3(x * hexWidth + xOff, 0, z * 3f / 4f * hexHeight);

        GameObject prefab;
        switch (type)
        {
            case MapCellTypes.Empty:
                prefab = GetRandomPrefab(emptyPrefabs);
                break;
            case MapCellTypes.Fence:
                prefab = GetRandomPrefab(fieldPrefabs);
                break;
            case MapCellTypes.Field:
                prefab = GetRandomPrefab(fieldPrefabs);
                break;
            case MapCellTypes.Barn:
                prefab = barnPrefab;
                break;
            default:
                prefab = null;
                break;
        }

        if (prefab == null) return;

        var hex = Instantiate(prefab, pos, Quaternion.identity);
    }

    private GameObject GetRandomPrefab(GameObject[] prefabs)
    {
        var index = UnityEngine.Random.Range(0, prefabs.Length);
        return prefabs[index];
    }
}
