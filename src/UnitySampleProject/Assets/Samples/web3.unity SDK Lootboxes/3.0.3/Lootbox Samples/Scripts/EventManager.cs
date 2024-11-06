using System;
using ChainSafe.Gaming.Evm.Transactions;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<ItemData[]> ToggleInventoryItems;
    public static event Action<TransactionReceipt> ToggleRewardItems;
    public static event Action<ItemData> ToggleNftModal;
    
    public static void OnToggleInventoryItems(ItemData[] items)
    {
        ToggleInventoryItems?.Invoke(items);
    }
    
    public static void OnToggleRewardItems(TransactionReceipt txReceipt)
    {
        ToggleRewardItems?.Invoke(txReceipt);
    }
    
    public static void OnToggleNftModal(ItemData items)
    {
        ToggleNftModal?.Invoke(items);
    }
}