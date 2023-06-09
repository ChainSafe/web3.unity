using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

public class AllErc721 : MonoBehaviour
{
    public string[] nftContracts;
    public int amountOfTokenIdsToSearch;
    public int tokenIDStart; // set this as a starting point as needed
    string account;
    int balanceSearched;

    async void Start()
    {
        // This is the account taken from the user login scene
        account = PlayerPrefs.GetString("Account");
        // Searches through your listed contracts
        foreach (string contract in nftContracts)
        {
            int balance = await ERC721.BalanceOf(contract, account);
            Debug.Log("Balance of contract " + contract + ": " + balance);
            // if i is less than the selected amount of tokenIDs to search, keep searching
            for (int i = 1; i < amountOfTokenIdsToSearch; i++)
            {
                // if balanceSearched is less than the balance for each contract, keep searching
                if (balanceSearched < balance)
                {
                    Debug.Log("Searching" + (tokenIDStart + i));
                    string ownerOf = await ERC721.OwnerOf(contract, (tokenIDStart + i).ToString());
                    // if token id id matches the account from login, print the tokenID and get the URI
                    if (ownerOf == account)
                    {
                        string uri = await ERC721.URI(contract, (tokenIDStart + i).ToString());
                        Debug.Log("TokenID: " + (tokenIDStart + i));
                        Debug.Log("Token URI: " + uri);
                        balanceSearched++;
                    }
                }
            }
        }
    }
}
