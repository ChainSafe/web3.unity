using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using Web3Unity.Scripts.Library.Ethers.Providers;

public class ERC721URIExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x06dc21f89f01409e7ed0e4c80eae1430962ae52c";
        string tokenId = "0x01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";

        string uri = await ERC721.URI(contract, tokenId);
        print(uri);
    }
}
