using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

public class ERC1155URIExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x2c1867BC3026178A47a677513746DCc6822A137A";
        string tokenId = "0x01559ae4021aee70424836ca173b6a4e647287d15cee8ac42d8c2d8d128927e5";
        string uri = await ERC1155.URI(contract, tokenId);
        print(uri);
    }
}
