using System;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<ItemData[]> ToggleInventoryItems;
    public static event Action<LootboxRewards> ToggleRewardItems;
    public static event Action<ItemData> ToggleNftModal;
    
    public static void OnToggleInventoryItems(ItemData[] items)
    {
        ToggleInventoryItems?.Invoke(items);
    }
    
    public static void OnToggleRewardItems(LootboxRewards rewards)
    {
        ToggleRewardItems?.Invoke(rewards);
    }
    
    public static void OnToggleNftModal(ItemData items)
    {
        ToggleNftModal?.Invoke(items);
    }
}