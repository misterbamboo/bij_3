using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] public GameObject menuInventory;

    public void BuyFence(int price)
    {
        BuyItem(ItemKeys.Fence, price);
    }

    public void BuyCampFire(int price)
    {
        BuyItem(ItemKeys.CampFire, price);
    }

    public void BuyTrap(int price)
    {
        BuyItem(ItemKeys.Trap, price);
    }

    public void BuyBees(int price)
    {
        BuyItem(ItemKeys.Bees, price);
    }

    private void BuyItem(string itemKey, int price)
    {
        print("price :" + price);
        if (MoneySys.BuyItem(price))
        {
            GameEvent.RaiseEvent(new ItemBoughtEvent(itemKey));
        }
        else
        {
            GameEvent.RaiseEvent(new NotEnoughtMoneyEvent());
        }
    }
}
