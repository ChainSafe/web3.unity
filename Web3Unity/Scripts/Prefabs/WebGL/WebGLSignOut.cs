using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WebGLSignOut : MonoBehaviour
{
    public void OnSignOut()
    {
        // Clear Account
        _Config.Account = "0x0000000000000000000000000000000000000001";
        // go to login scene
        SceneManager.LoadScene(0);
    }
}
