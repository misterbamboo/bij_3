using System;
using UnityEngine;

public class SideEffectManager : MonoBehaviour
{
    public static SideEffectManager Instance { get; private set; }

    [Header("Per item purchased")]
    [SerializeField] private float SpawnAugmentationPerItem = 0.2f;
    [SerializeField] private float SheepSpeedAugmentationPerItem = 1;
    [SerializeField] private float VisionReductionPerItem = 0.1f;

    [Header("Current Rates")]
    [SerializeField] private float CurrentSpawnRate = 1;
    [SerializeField] private float CurrentSheepSpeed = 1;
    [SerializeField] private float CurrentVision = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameEvent.Subscribe<ItemBoughtEvent>(OnItemBought);
    }

    private void OnItemBought(ItemBoughtEvent evnt)
    {
        switch (evnt.ItemKey)
        {
            case ItemKeys.Trap:
                CurrentSpawnRate += SpawnAugmentationPerItem;
                break;
            case ItemKeys.CampFire:
                CurrentSheepSpeed += SheepSpeedAugmentationPerItem;
                break;
            case ItemKeys.Bees:
                CurrentVision -= VisionReductionPerItem;
                break;
            case ItemKeys.Fence:
            default:
                // Nothing
                break;
        }
    }

    public float GetSpawnInSecs()
    {
        return 1 / CurrentSpawnRate;
    }

    public float GetSheepSpeed()
    {
        return CurrentSheepSpeed;
    }

    public float GetVision()
    {
        return CurrentVision;
    }
}
