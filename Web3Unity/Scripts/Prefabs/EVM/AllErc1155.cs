using System.Numerics;
using UnityEngine;

public class AllErc1155 : MonoBehaviour
{
    string account;
    public string chain;
    public string network;
    public string tokenIdHex;
    public string[] nftContracts = {};

    async void Start()
    {
        // This is the account taken from the user login scene
        account = PlayerPrefs.GetString("Account");
        // Searches through your listed contracts for balance and uri of the chosen tokenId
        foreach (string contract in nftContracts)
        {
            BigInteger balance = await ERC1155.BalanceOf(chain, network, contract, account, tokenIdHex, "");
            Debug.Log("Balance of contract " + contract + ": " + balance);
            string uri = await ERC1155.URI(chain, network, contract, tokenIdHex);
            Debug.Log("Token URI: " + uri);
        }
    }
}