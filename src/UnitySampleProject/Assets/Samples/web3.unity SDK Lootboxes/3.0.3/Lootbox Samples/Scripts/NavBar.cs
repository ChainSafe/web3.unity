using UnityEngine;
using UnityEngine.UI;

public class NavBar : MonoBehaviour
{

    [SerializeField] private Button openLootboxButton, myInventoryButton;
    [SerializeField] private GameObject openLootboxMenu, myInventoryMenu;
    
    private void Awake()
    {
        openLootboxButton.onClick.AddListener(() => OpenMenu(openLootboxMenu));
        myInventoryButton.onClick.AddListener(() => OpenMenu(myInventoryMenu));
    }
    
    private void OpenMenu(GameObject menuToOpen)
    {
        CloseAllMenus();
        menuToOpen.SetActive(true);
    }
    
    private void CloseAllMenus()
    {
        openLootboxMenu.SetActive(false);
        myInventoryMenu.SetActive(false);
    }
}
