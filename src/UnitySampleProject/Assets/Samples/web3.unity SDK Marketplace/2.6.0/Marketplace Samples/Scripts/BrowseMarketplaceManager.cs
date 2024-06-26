using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the marketplace browse GUI.
/// </summary>
public class BrowseMarketplaceManager : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private GameObject selectMarketplaceMenu;
    [SerializeField] private GameObject browseMarketplaceMenu;
    [SerializeField] private GameObject marketplaceItemPrefab;
    [SerializeField] private Button openSelectMarketplaceOptionButton;
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// Initializes objects.
    /// </summary>
    private void Awake()
    {
        openSelectMarketplaceOptionButton.onClick.AddListener(OpenSelectMarketplaceOptionMenu);
    }
    
    /// <summary>
    /// Opens the select marketplace option menu.
    /// </summary>
    private void OpenSelectMarketplaceOptionMenu()
    {
        browseMarketplaceMenu.SetActive(false);
        selectMarketplaceMenu.SetActive(true);
    }

    #endregion
}
