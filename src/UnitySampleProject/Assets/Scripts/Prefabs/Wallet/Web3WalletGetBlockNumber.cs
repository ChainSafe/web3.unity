using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

public class Web3WalletGetBlockNumber : MonoBehaviour
{
    public async void GetBlockNumber()
    {
        var provider = ProviderMigration.NewJsonRpcProviderAsync("YOUR_NODE").Result;
        Debug.Log("Block Number: " + await provider.GetBlockNumber());
    }
}