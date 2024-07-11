using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using AOT;
using ChainSafe.Gaming;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.GamingSdk.Web3Auth;
using Nethereum.Hex.HexTypes;
using UnityEngine;
using Network = Web3Auth.Network;

public class Web3AuthConnectionProvider : ConnectionProvider
{
    public override bool IsAvailable { get; }

    public override Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        throw new NotImplementedException();
    }
}
