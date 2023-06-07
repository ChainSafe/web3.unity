using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

public class Web3WalletGetGasPrice : MonoBehaviour
{
    public async void GetGasPrice()
    {
        var provider = ProviderMigration.NewJsonRpcProviderAsync("YOUR_NODE").Result;
        Debug.Log("Gas Price: " + await provider.GetGasPrice());
    }
}