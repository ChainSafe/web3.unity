using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

public class Web3WalletGetGasPrice : MonoBehaviour
{
    public void GetGasPrice()
    {
        // todo: can't use Task.Result here, and this code has to conform to the new interfaces anyway
        //var provider = ProviderMigration.NewJsonRpcProviderAsync("YOUR_NODE").Result;
        //Debug.Log("Gas Price: " + await provider.GetGasPrice());
    }
}