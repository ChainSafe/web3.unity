using System;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using UnityEngine;

public class GoerliCheck : MonoBehaviour
{
    void Start()
    {
        if (Web3Accessor.Web3.ChainConfig.ChainId != "5")
        {
            throw new Web3Exception("Please set your chain to Goerli to see the examples functioning correctly");
        }
    }
}
