using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;

public class GetBlockNumber : MonoBehaviour
{
    public async void Start()
    {
        Debug.Log("Block Number: " + await Web3Accessor.Web3.RpcProvider.GetBlockNumber());
    }
}