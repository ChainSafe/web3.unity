using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
    [SerializeField] List<GameObject> testMenus = new List<GameObject>();
    [SerializeField] GameObject closeButton;

    public void OpenRelatedMenu(GameObject menu)
    {
        OpenCloseMenus(menu, true);
    }
    public void ReturnToMainMenu(GameObject menu)
    {
        OpenCloseMenus(menu, false);
    }
    private void OpenCloseMenus(GameObject menu, bool isCloseButtonEnabled)
    {
        for (int i = 0; i < testMenus.Count; i++) {
            testMenus[i].SetActive(false);
        }
        menu.SetActive(true);
        closeButton.SetActive(isCloseButtonEnabled);
    }
}
