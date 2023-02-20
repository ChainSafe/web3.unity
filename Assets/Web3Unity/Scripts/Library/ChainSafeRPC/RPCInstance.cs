using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Prefabs.Ethers;


public sealed class RPC
{
    private static RPC instance = null;
    private static JsonRpcProvider provider;
    public static RPC GetInstance
    {
        get
        {
            if (instance == null)
                instance = new RPC();
            return instance;
        }
    }

    private RPC()
    {
        provider = new JsonRpcProvider(PlayerPrefs.GetString("RPC"));
    }

    public JsonRpcProvider Provider()
    {
        return provider;
    }
}
