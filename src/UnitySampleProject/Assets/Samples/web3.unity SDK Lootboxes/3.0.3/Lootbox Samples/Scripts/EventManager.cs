using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<ItemData[]> ToggleInventoryItems;
    public static event Action<ItemData> ToggleNftModal;
    
    public static void OnToggleInventoryItems(ItemData[] items)
    {
        ToggleInventoryItems?.Invoke(items);
    }
    
    public static void OnToggleNftModal(ItemData items)
    {
        ToggleNftModal?.Invoke(items);
    }
}