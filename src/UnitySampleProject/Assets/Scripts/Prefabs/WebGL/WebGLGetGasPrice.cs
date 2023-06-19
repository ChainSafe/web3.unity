using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

#if UNITY_WEBGL
public class WebGLGetGasPrice : MonoBehaviour
{
    public async void GetGasPrice()
    {
        // todo: can't use Task.Result here, and this code has to conform to the new interfaces anyway
        //var provider = ProviderMigration.NewJsonRpcProviderAsync("YOUR_NODE").Result;
        //Debug.Log("Gas Price: " + await provider.GetGasPrice());
    }
}
#endif