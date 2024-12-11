using System;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using UnityEngine;

/// <summary>
/// Event manager class to help with displaying lootbox inventory items.
/// </summary>
public class EventManager : MonoBehaviour
{
    #region Fields

    public static EventManager Instance { get; private set; }
    public event Action<ItemData[]> ToggleInventoryItems;
    public event Action<LootboxRewards> ToggleRewardItems;
    public event Action<ItemData> ToggleNftModal;
    public event Action<ItemData> ToggleNftData;

    #endregion

    #region Methods
    
    /// <summary>
    /// Initializes instance.
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Toggles inventory items with item data.
    /// </summary>
    /// <param name="items">Array of inventory item data.</param>
    public void OnToggleInventoryItems(ItemData[] items)
    {
        ToggleInventoryItems?.Invoke(items);
    }

    /// <summary>
    /// Toggles reward items with LootboxRewards data.
    /// </summary>
    /// <param name="items">List of lootbox reward data.</param>
    public void OnToggleRewardItems(LootboxRewards rewards)
    {
        ToggleRewardItems?.Invoke(rewards);
    }

    /// <summary>
    /// Toggles the NFT modal with item data.
    /// </summary>
    /// <param name="items">Selected NFT item data.</param>
    public void OnToggleNftModal(ItemData items)
    {
        ToggleNftModal?.Invoke(items);
    }

    /// <summary>
    /// Toggles NFT modal data.
    /// </summary>
    /// <param name="items">Selected NFT item data</param>
    public void OnToggleNftData(ItemData items)
    {
        ToggleNftData?.Invoke(items);
    }

    #endregion
}