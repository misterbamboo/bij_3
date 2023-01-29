using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacingItemManager : MonoBehaviour
{
    [SerializeField] float hexSize = 3;
    [SerializeField] GameObject fencePrefab;
    [SerializeField] GameObject trapPrefab;
    [SerializeField] GameObject beeHomePrefab;
    [SerializeField] GameObject campFirePrefab;

    private int placeItemPlaneLayerMask;
    private float hexWidth;
    private float hexHeight;
    private float hexHeightPerIndex;
    private int selectionApproxZIndex;
    private int selectionApproxXIndex;
    private PlaceItemAnchor closestAnchor;

    private string ItemToPlace { get; set; }
    private GameObject GameObjectToPlace { get; set; }
    public MapCellCoord LastHighlighCoord { get; private set; }

    private void Start()
    {
        GameEvent.Subscribe<ItemBoughtEvent>(ItemBoughtHandler);
        placeItemPlaneLayerMask = LayerMask.GetMask("PlaceItemPlane");

        hexWidth = Mathf.Sqrt(3) * hexSize;
        hexHeight = hexSize * 2;
        hexHeightPerIndex = 3f / 4f * hexHeight;
    }

    private void ItemBoughtHandler(ItemBoughtEvent evnt)
    {
        ItemToPlace = evnt.ItemKey;
        switch (ItemToPlace)
        {
            case ItemKeys.Fence:
                GameObjectToPlace = Instantiate(fencePrefab);
                break;
            case ItemKeys.Trap:
                GameObjectToPlace = Instantiate(trapPrefab);
                break;
            case ItemKeys.Bees:
                GameObjectToPlace = Instantiate(beeHomePrefab);
                break;
            case ItemKeys.CampFire:
                GameObjectToPlace = Instantiate(campFirePrefab);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        selectionApproxXIndex = -1000;
        selectionApproxZIndex = -1000;

        if (!string.IsNullOrEmpty(ItemToPlace))
        {
            MoveItemToPointerPosition();
            HighlightHexCell();
            CheckIfCanPlace();
        }
    }

    private void MoveItemToPointerPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, placeItemPlaneLayerMask))
        {
            //var xOff = (zCoord % 2) * (hexWidth / 2f);
            //var pos = new Vector3(x * hexWidth + xOff, 0, zCoord * );

            selectionApproxZIndex = (int)(hit.point.z / hexHeightPerIndex);

            var xOff = ((selectionApproxZIndex - 1) % 2) * (hexWidth / 2);
            selectionApproxXIndex = (int)((hit.point.x - xOff) / hexWidth);

            float closedDistance = float.MaxValue;
            foreach (var anchor in GetNeerbyAnchors())
            {
                var distance = (anchor.Position - hit.point).magnitude;
                if (distance < closedDistance)
                {
                    closedDistance = distance;
                    closestAnchor = anchor;
                }
            }

            GameObjectToPlace.transform.position = new Vector3(closestAnchor.Position.x, hit.point.y, closestAnchor.Position.z);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var anchor in GetNeerbyAnchors())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(anchor.Position, 0.5f);
        }
    }

    private IEnumerable<PlaceItemAnchor> GetNeerbyAnchors()
    {
        for (int xInd = selectionApproxXIndex - 1; xInd <= selectionApproxXIndex + 1; xInd++)
        {
            for (int zInd = selectionApproxZIndex - 1; zInd <= selectionApproxZIndex + 1; zInd++)
            {
                var zPos = zInd * hexHeightPerIndex;

                var xOff = (zInd % 2) * hexWidth / 2;
                var xPos = xInd * hexWidth + xOff;

                var anchor = new PlaceItemAnchor(xInd, zInd, new Vector3(xPos, 2, zPos));
                yield return anchor;
            }
        }
    }

    private void HighlightHexCell()
    {
        var map = MapGenerator.Instance.GetMap();
        MapCellCoord coord = GetPointedCoord();

        if (!LastHighlighCoord.Equals(LastHighlighCoord, coord))
        {
            LastHighlighCoord = coord;

            var isBlocked = map.IsBlocked(coord);
            GameEvent.RaiseEvent(new HexCellHoverEvent(coord, isBlocked));
        }
    }

    private void CheckIfCanPlace()
    {
        if (Input.GetMouseButton(0))
        {
            var map = MapGenerator.Instance.GetMap();

            MapCellCoord coord = GetPointedCoord();
            var type = map.GetMapCellType(coord);

            // Debuging indexes => to => hexCoord
            // print($"{closestAnchor.XIndex},{closestAnchor.ZIndex}   ===>   {xCoord},{zCoord}   ({type})");

            if (type == MapCellTypes.Field || type == MapCellTypes.Empty)
            {
                PlaceItem(coord, map);
            }
        }
    }

    private MapCellCoord GetPointedCoord()
    {
        int zCoord, xCoord;
        if (closestAnchor.ZIndex % 2 == 0)
        {
            zCoord = closestAnchor.ZIndex + ((closestAnchor.ZIndex % 2) * -1);
            xCoord = (closestAnchor.XIndex + (closestAnchor.ZIndex % 2)) * 2;
        }
        else
        {
            zCoord = closestAnchor.ZIndex;
            xCoord = (closestAnchor.XIndex * 2) + 1;
        }
        return new MapCellCoord(xCoord, zCoord);
    }

    private void PlaceItem(MapCellCoord coord, Map map)
    {
        ItemToPlace = null;
        GameObjectToPlace = null;
        closestAnchor = new PlaceItemAnchor();
        map.SetMapCellType(coord, MapCellTypes.Turret);
        GameEvent.RaiseEvent(new ItemPlacedEvent());
    }
}

