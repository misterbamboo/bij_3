using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapDrawer : MonoBehaviour
{
    [SerializeField] GameObject[] fieldPrefabs;
    [SerializeField] GameObject[] fencePrefabs;
    [SerializeField] GameObject[] dirtPrefabs;
    [SerializeField] GameObject fenceLinkPrefab;
    [SerializeField] GameObject barnPrefab;

    [Header("Outiline materials")]
    [SerializeField] Material greenOutiline;
    [SerializeField] Material redOutline;

    [SerializeField] float hexSize = 1;
    private float hexWidth;
    private float hexHeight;

    private Map Map { get; set; }

    private Dictionary<MapCellCoord, MeshRenderer> hexCellsRenderers = new Dictionary<MapCellCoord, MeshRenderer>();
    private MeshRenderer currentHoverCellRenderer;
    private int currentHoverCellMaterialIndex = -1;

    private void Start()
    {
        GameEvent.Subscribe<HexCellHoverEvent>(OnHexCellHover);
        GameEvent.Subscribe<ItemPlacedEvent>(OnItemPlaced);
    }

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

        KeepHexMeshRendererCash(xCoord, zCoord, hex);
    }

    private void KeepHexMeshRendererCash(int xCoord, int zCoord, GameObject hex)
    {
        // Most of hexCell have meshRenderer on Prefab root
        var meshRend = hex.GetComponent<MeshRenderer>();
        if (meshRend == null)
        {
            // Fences have meshRenderer one of the first child of the Prefab root
            var currentTransfort = hex.transform;
            while (currentTransfort.childCount > 0)
            {
                var firstChild = currentTransfort.GetChild(0);
                meshRend = firstChild.GetComponent<MeshRenderer>();
                if (meshRend != null)
                {
                    break;
                }
                currentTransfort = firstChild.transform;
            }
        }

        if (meshRend != null)
        {
            hexCellsRenderers[new MapCellCoord(xCoord, zCoord)] = meshRend;
        }
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

    private void OnHexCellHover(HexCellHoverEvent evnt)
    {
        if (!hexCellsRenderers.ContainsKey(evnt.Coord)) return;

        CleanHoverOutiline();

        var meshRenderer = hexCellsRenderers[evnt.Coord];
        var currentMats = meshRenderer.materials.ToList();
        var outlineMaterial = evnt.IsBlocked ? redOutline : greenOutiline;
        currentMats.Add(outlineMaterial);
        meshRenderer.materials = currentMats.ToArray();

        currentHoverCellRenderer = meshRenderer;
        currentHoverCellMaterialIndex = meshRenderer.materials.Length - 1;
    }


    private void OnItemPlaced(ItemPlacedEvent obj)
    {
        CleanHoverOutiline();
    }

    private void CleanHoverOutiline()
    {
        if (currentHoverCellMaterialIndex >= 0)
        {
            var mats = currentHoverCellRenderer.materials.ToList();
            mats.RemoveAt(currentHoverCellMaterialIndex);
            currentHoverCellRenderer.materials = mats.ToArray();
            currentHoverCellRenderer = null;
            currentHoverCellMaterialIndex = -1;
        }
    }
}
