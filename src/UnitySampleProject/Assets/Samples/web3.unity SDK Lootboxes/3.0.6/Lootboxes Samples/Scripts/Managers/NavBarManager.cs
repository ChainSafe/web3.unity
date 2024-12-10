using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Nav bar config for lootboxes.
/// </summary>
public class NavBarManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private Button openLootboxButton, myInventoryButton;
    [SerializeField] private GameObject openLootboxMenu, myInventoryMenu;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes button & functions.
    /// </summary>
    private void Awake()
    {
        openLootboxButton.onClick.AddListener(() => OpenMenu(openLootboxMenu));
        myInventoryButton.onClick.AddListener(() => OpenMenu(myInventoryMenu));
    }

    /// <summary>
    /// Assists with opening and closing menus.
    /// </summary>
    /// <param name="menuToOpen">The menu object to open.</param>
    private void OpenMenu(GameObject menuToOpen)
    {
        CloseAllMenus();
        menuToOpen.SetActive(true);
    }

    /// <summary>
    /// Closes all menus.
    /// </summary>
    private void CloseAllMenus()
    {
        openLootboxMenu.SetActive(false);
        myInventoryMenu.SetActive(false);
    }

    #endregion
}