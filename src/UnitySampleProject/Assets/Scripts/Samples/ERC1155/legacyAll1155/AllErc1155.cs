using System.Numerics;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

public class AllErc1155 : MonoBehaviour
{
    string account;
    public string tokenIdHex;
    public string[] nftContracts;

    async void Start()
    {
        // This is the account taken from the user login scene
        account = await Web3Accessor.Web3.Signer.GetAddress();
        // Searches through your listed contracts for balance and uri of the chosen tokenId
        foreach (string contract in nftContracts)
        {
            BigInteger balance = await ERC1155.BalanceOf(Web3Accessor.Web3, contract, account, tokenIdHex);
            Debug.Log("Balance of contract " + contract + ": " + balance);
            string uri = await ERC1155.URI(Web3Accessor.Web3, contract, tokenIdHex);
            Debug.Log("Token URI: " + uri);
        }
    }
}