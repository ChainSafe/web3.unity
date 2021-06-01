using System.Numerics;
using UnityEngine;

public class EthereumBroadcastExample : MonoBehaviour
{
    async void Start()
    {
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string signedTransaction = "0xf86b04843b9aca0083989680947e3be66431ba73956213c40c0828355d1a7894d38703f18a03b36000802ca0043ab6289f2a44dd911bfb3658cfac12710354a3e0cef35544c9348b15f9f6f7a018d36b8d5b61dc00a54293528d0edd8a4a7c9f064817825c7e8cb8167b240860";

        string transactionHash = await Ethereum.Broadcast(network, signedTransaction);
        
        print (transactionHash);
    }
}
