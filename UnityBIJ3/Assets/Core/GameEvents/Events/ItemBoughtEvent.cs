public class ItemBoughtEvent : IGameEventBase
{
    public string ItemKey { get; }

    public ItemBoughtEvent(string itemKey)
    {
        ItemKey = itemKey;
    }
}
