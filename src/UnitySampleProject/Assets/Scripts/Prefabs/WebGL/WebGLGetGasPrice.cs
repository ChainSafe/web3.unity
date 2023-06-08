using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

#if UNITY_WEBGL
public class WebGLGetGasPrice : MonoBehaviour
{
    public async void GetGasPrice()
    {
        var provider = ProviderMigration.NewJsonRpcProvider("YOUR_NODE");
        Debug.Log("Gas Price: " + await provider.GetGasPrice());
    }
}
#endif