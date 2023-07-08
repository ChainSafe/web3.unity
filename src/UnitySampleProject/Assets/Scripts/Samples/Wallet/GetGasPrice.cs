using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

public class GetGasPrice : MonoBehaviour
{
    public async void Start()
    {
        Debug.Log("Gas Price: " + await Web3Accessor.Web3.RpcProvider.GetGasPrice());
    }
}