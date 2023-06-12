using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

public class Web3WalletGetBlockNumber : MonoBehaviour
{
    public async void GetBlockNumber()
    {
        var provider = ProviderMigration.NewJsonRpcProvider("YOUR_NODE");
        Debug.Log("Block Number: " + await provider.GetBlockNumber());
    }
}