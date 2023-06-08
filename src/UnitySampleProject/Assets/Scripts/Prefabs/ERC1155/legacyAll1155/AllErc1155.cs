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
        account = PlayerPrefs.GetString("Account");
        // Searches through your listed contracts for balance and uri of the chosen tokenId
        foreach (string contract in nftContracts)
        {
            BigInteger balance = await ERC1155.BalanceOf(contract, account, tokenIdHex);
            Debug.Log("Balance of contract " + contract + ": " + balance);
            string uri = await ERC1155.URI(contract, tokenIdHex);
            Debug.Log("Token URI: " + uri);
        }
    }
}