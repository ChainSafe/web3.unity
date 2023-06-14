using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

#if UNITY_WEBGL
public class WebGLGetBlockNumber : MonoBehaviour
{
    public async void GetBlockNumber()
    {
        // todo: can't use Task.Result here, and this code has to conform to the new interfaces anyway
        //var provider = ProviderMigration.NewJsonRpcProviderAsync("YOUR_NODE").Result;
        //Debug.Log("Block Number: " + await provider.GetBlockNumber());
    }
}
#endif