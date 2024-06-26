using System.Collections.Generic;
using ChainSafe.Gaming.Marketplace;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the marketplace selection system.
/// </summary>
public class MarketplaceManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject marketplaceLoginPrefab;
    [SerializeField] private Button logOutButton;
    [SerializeField] private List<GameObject> primaryBackgroundObjects;
    [SerializeField] private List<GameObject> menuBackgroundObjects;
    [SerializeField] private List<GameObject> primaryTextObjects;
    [SerializeField] private List<GameObject> secondaryTextObjects;
    [SerializeField] private List<GameObject> displayLineObjects;
    [SerializeField] private List<GameObject> borderButtonObjects;

    #endregion

    #region Methods
    
    /// <summary>
    /// Initializes objects.
    /// </summary>
    private void Awake()
    {
        logOutButton.onClick.AddListener(MarketplaceLogout);
        SetCustomColours();
    }
    
    /// <summary>
    /// Sets custom colours.
    /// </summary>
    private void SetCustomColours()
    {
        CustomizationHelperMarketplace.SetCustomColours(
            EventManagerMarketplace.MarketplaceGUIConfigEventArgs.DisplayFont,
            primaryBackgroundObjects, EventManagerMarketplace.MarketplaceGUIConfigEventArgs.PrimaryBackgroundColour,
            menuBackgroundObjects, EventManagerMarketplace.MarketplaceGUIConfigEventArgs.MenuBackgroundColour,
            primaryTextObjects, EventManagerMarketplace.MarketplaceGUIConfigEventArgs.PrimaryTextColour,
            secondaryTextObjects, EventManagerMarketplace.MarketplaceGUIConfigEventArgs.SecondaryTextColour,
            borderButtonObjects, EventManagerMarketplace.MarketplaceGUIConfigEventArgs.BorderButtonColour,
            displayLineObjects);
    }
    
    /// <summary>
    /// Logs out of the marketplace.
    /// </summary>
    private void MarketplaceLogout()
    {
        Instantiate(marketplaceLoginPrefab);
        Destroy(gameObject);
    }

    #endregion
    
}
