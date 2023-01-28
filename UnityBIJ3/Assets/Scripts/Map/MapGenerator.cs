using System;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int mapSize = 50;
    [SerializeField] float xScale = 1;
    [SerializeField] float zScale = 1;
    [SerializeField] float xOffset = 0;
    [SerializeField] float zOffset = 0;
    [SerializeField] float yOffset = 0;
    [SerializeField] float height = 1;

    [SerializeField] int centerPosX = 25;
    [SerializeField] int centerPosZ = 25;
    [SerializeField] float centerRadius = 50;
    [SerializeField] float centerMultiplier = 5;

    Map map;
    float[,] perlinMap;

    string generationKey = string.Empty;
    private bool isGizmos;

    private void OnDrawGizmos()
    {
        isGizmos = true;
        CheckInit();
    }

    private void OnDrawGizmosSelected()
    {
        isGizmos = true;
        CheckInit();
    }

    private void Start()
    {
        CheckInit();
    }

    void Update()
    {
        CheckInit();
    }

    private void CheckInit()
    {
        var currentGenerationKey = $"{mapSize};{xScale};{zScale};{xOffset};{zOffset};{yOffset};{height};{centerPosX};{centerPosZ};{centerRadius};{centerMultiplier}";
        if (generationKey != currentGenerationKey)
        {
            perlinMap = new float[mapSize * 2, mapSize];
            map = new Map(mapSize);
            generationKey = currentGenerationKey;
            RegenerateMap();
        }
    }

    private void RegenerateMap()
    {
        UpdatePerlinMap();
        SetMapCells();
        CleanFences();
        var mapGizmos = GetComponent<MapGizmos>();
        if (mapGizmos != null)
        {
            mapGizmos.Refresh(map);
        }

        if (!isGizmos)
        {
            var mapDrawer = GetComponent<MapDrawer>();
            if (mapDrawer != null)
            {
                mapDrawer.Refresh(map);
            }
        }
    }

    private void UpdatePerlinMap()
    {
        for (int zCoord = 0; zCoord < mapSize; zCoord++)
        {
            float blurValue = 0;
            for (int x = 0; x < mapSize * 2; x += 2)
            {
                int z = zCoord;
                int xCoord = x + (zCoord % 2);

                var normalValue = Mathf.PerlinNoise((xOffset + xCoord) / mapSize * xScale, (zOffset + z) / mapSize * zScale);

                var value = normalValue;
                var distance = (new Vector2(x, z) - new Vector2(centerPosX, centerPosZ)).magnitude;
                if (distance < centerRadius)
                {
                    var t = (1 - distance / centerRadius);
                    value *= (t * centerMultiplier);
                    value = Mathf.SmoothStep(normalValue, value, t);
                }

                var currentBlurValue = blurValue;
                blurValue = value;
                value = (currentBlurValue + value) / 2;
                perlinMap[xCoord, zCoord] = Mathf.Clamp((value * height) + yOffset, 0, float.MaxValue);
            }
        }
    }

    private void SetMapCells()
    {
        for (int zCoord = 0; zCoord < mapSize; zCoord++)
        {
            for (int x = 0; x < mapSize * 2; x += 2)
            {
                int xCoord = x + (zCoord % 2);

                if (perlinMap[xCoord, zCoord] > 0)
                {
                    var mapCell = HasEmptyCellArround(xCoord, zCoord) ? MapCellTypes.Fence : MapCellTypes.Field;
                    if (xCoord == centerPosX && zCoord == centerPosZ)
                    {
                        mapCell = MapCellTypes.Barn;
                    }
                    map.SetMapCellType(new MapCellCoord(xCoord, zCoord), mapCell);
                }
            }
        }
    }

    private void CleanFences()
    {
        for (int zCoord = 0; zCoord < mapSize; zCoord++)
        {
            for (int x = 0; x < mapSize * 2; x += 2)
            {
                int xCoord = x + (zCoord % 2);
                var type = map.GetMapCellType(xCoord, zCoord);
                if (type == MapCellTypes.Fence)
                {
                    if (FenceShouldBeCleanned(xCoord, zCoord))
                    {
                        map.SetMapCellType(new MapCellCoord(xCoord, zCoord), MapCellTypes.Empty);
                    }
                }
            }
        }
    }

    private bool FenceShouldBeCleanned(int x, int z)
    {
        return
            SurroundedByFenceOrEmpty(x - 2, z) &&
            SurroundedByFenceOrEmpty(x + 2, z) &&
            SurroundedByFenceOrEmpty(x - 1, z - 1) &&
            SurroundedByFenceOrEmpty(x - 1, z + 1) &&
            SurroundedByFenceOrEmpty(x + 1, z - 1) &&
            SurroundedByFenceOrEmpty(x + 1, z + 1);
    }

    private bool SurroundedByFenceOrEmpty(int x, int z)
    {
        var type = map.GetMapCellType(x, z);
        return type == MapCellTypes.Fence || type == MapCellTypes.Empty;
    }

    private bool HasEmptyCellArround(int x, int z)
    {
        return
            EmptyAt(x - 2, z) ||
            EmptyAt(x + 2, z) ||
            EmptyAt(x - 1, z - 1) ||
            EmptyAt(x - 1, z + 1) ||
            EmptyAt(x + 1, z - 1) ||
            EmptyAt(x + 1, z + 1);
    }

    private bool EmptyAt(int x, int z)
    {
        if (x < 0 || z < 0) return true;
        if (x >= perlinMap.GetLength(0) || z >= perlinMap.GetLength(1)) return true;

        return perlinMap[x, z] <= 0;
    }
}