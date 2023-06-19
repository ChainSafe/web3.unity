using System;
using System.Threading.Tasks;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

public sealed class RPC
{
    private static RPC instance = null;
    private JsonRpcProvider provider;

    public static RPC GetInstance => instance ?? throw new Exception("RPC instance is not initialized");

    public static async Task InitializeInstance()
    {
        instance = new();
        instance.provider = await ProviderMigration.NewJsonRpcProviderAsync();
    }

    public JsonRpcProvider Provider() => provider;
}
