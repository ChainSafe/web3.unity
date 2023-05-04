using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

public class Web3WalletGetGasPrice : MonoBehaviour
{
    public async void GetGasPrice()
    {
        var provider = ProviderMigration.NewJsonRpcProvider("YOUR_NODE");
        Debug.Log("Gas Price: " + await provider.GetGasPrice());
    }
}