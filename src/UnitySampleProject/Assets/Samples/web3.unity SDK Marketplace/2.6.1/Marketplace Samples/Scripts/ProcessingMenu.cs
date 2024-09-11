using System;
using UnityEngine;

public class ProcessingMenu : MonoBehaviour
{
    private static ProcessingMenu _instance;

    public static ProcessingMenu Instance
    {
        get
        {
            if (_instance != null)
                return _instance;
            _instance = FindObjectOfType<ProcessingMenu>(true);
            if (_instance == null)
            {
                throw new InvalidOperationException(
                    "Trying to find Processing Menu but couldn't find one, make sure one processing menu exists in the scene");
            }
            return _instance;
        }
    }
    
    public static void ToggleMenu()
    {
        Instance.gameObject.SetActive(!Instance.gameObject.activeInHierarchy);
    }
}
