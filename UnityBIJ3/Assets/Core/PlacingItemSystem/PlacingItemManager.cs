using UnityEngine;

public class PlacingItemManager : MonoBehaviour
{
    public static PlacingItemManager Instance { get; private set; }

    public PlacingItemUtils PlacingItemUtils => placingItemUtils;

    [SerializeField] float hexSize = 3;
    [SerializeField] GameObject fencePrefab;
    [SerializeField] GameObject trapPrefab;
    [SerializeField] GameObject beeHomePrefab;
    [SerializeField] GameObject campFirePrefab;

    private int placeItemPlaneLayerMask;
    private float hexWidth;
    private float hexHeight;
    private float hexHeightPerIndex;
    private PlacingItemUtils placingItemUtils;

    private string ItemToPlace { get; set; }
    private GameObject GameObjectToPlace { get; set; }
    public MapCellCoord LastHighlighCoord { get; private set; }
    private SnapPosition currentAnchor = new SnapPosition();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameEvent.Subscribe<ItemBoughtEvent>(ItemBoughtHandler);
        placeItemPlaneLayerMask = LayerMask.GetMask("PlaceItemPlane");

        hexWidth = Mathf.Sqrt(3) * hexSize;
        hexHeight = hexSize * 2;
        hexHeightPerIndex = 3f / 4f * hexHeight;

        placingItemUtils = new PlacingItemUtils(hexHeightPerIndex, hexWidth);
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

            var closestAnchor = placingItemUtils.FindClosestSnapPosition(hit.point);

            GameObjectToPlace.transform.position = new Vector3(closestAnchor.Position.x, hit.point.y, closestAnchor.Position.z);
            currentAnchor = closestAnchor;
        }
    }

    private void OnDrawGizmos()
    {
        if (GameObjectToPlace != null)
        {
            var approx = placingItemUtils.GetApproximativeXZIndex(GameObjectToPlace.transform.position);
            foreach (var anchor in placingItemUtils.GetNeerbySnapPositions(approx))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(anchor.Position, 0.5f);
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
        if (currentAnchor.ZIndex % 2 == 0)
        {
            zCoord = currentAnchor.ZIndex + ((currentAnchor.ZIndex % 2) * -1);
            xCoord = (currentAnchor.XIndex + (currentAnchor.ZIndex % 2)) * 2;
        }
        else
        {
            zCoord = currentAnchor.ZIndex;
            xCoord = (currentAnchor.XIndex * 2) + 1;
        }
        return new MapCellCoord(xCoord, zCoord);
    }

    private void PlaceItem(MapCellCoord coord, Map map)
    {
        switch (ItemToPlace)
        {
            case ItemKeys.Trap:
                GameObjectToPlace.GetComponent<WolfTrap>().isActive = true;
                break;
            case ItemKeys.Bees:
                GameObjectToPlace.GetComponentInChildren<Bee>().isActive = true;
                break;
            case ItemKeys.CampFire:
                GameObjectToPlace.GetComponent<AOEItem>().isActive = true;
                break;
            default:
                break;
        }


        ItemToPlace = null;
        GameObjectToPlace = null;
        currentAnchor = new SnapPosition();
        map.SetMapCellType(coord, MapCellTypes.Turret);
        GameEvent.RaiseEvent(new ItemPlacedEvent());
    }
}

