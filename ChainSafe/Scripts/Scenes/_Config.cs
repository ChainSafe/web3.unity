using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Config : MonoBehaviour
{
    // this will store configs between scenes

    // account will be updated once user logs in
    public static string Account = "0x0000000000000000000000000000000000000001";

    // host url for API
    public readonly static string Host = "https://restx-ethereum.herokuapp.com";

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
