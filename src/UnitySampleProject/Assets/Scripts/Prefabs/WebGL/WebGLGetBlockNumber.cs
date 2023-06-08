using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

#if UNITY_WEBGL
public class WebGLGetBlockNumber : MonoBehaviour
{
    public async void GetBlockNumber()
    {
        var provider = ProviderMigration.NewJsonRpcProviderAsync("YOUR_NODE").Result;
        Debug.Log("Block Number: " + await provider.GetBlockNumber());
    }
}
#endif