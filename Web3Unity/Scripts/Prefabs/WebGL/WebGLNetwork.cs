using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_WEBGL
public class WebGLNetwork : MonoBehaviour
{
    public void OnGetNetwork()
    {
        // list of networks here: https://chainlist.org/
        int network = Web3GL.Network();
        print("User's current network: " + network);
    }
}
#endif