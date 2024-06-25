using System;
using UnityEngine;

public class DisableObjectForPlatform : MonoBehaviour
{
    [SerializeField]
    private RuntimePlatform platform;

    private void Awake()
    {
        if (Application.platform == platform)
        {
            gameObject.SetActive(false);
        }
    }
}
