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

    #endregion

    #region Methods
    
    /// <summary>
    /// Initializes objects.
    /// </summary>
    private void Awake()
    {
        logOutButton.onClick.AddListener(MarketplaceLogout);
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
