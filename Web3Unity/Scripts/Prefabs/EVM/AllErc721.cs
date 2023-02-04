using UnityEngine;

public class AllErc721 : MonoBehaviour
{
    string account;
    int balanceSearched;
    public string chain;
    public string network;
    public int amountOfTokenIdsToSearch;
    public int tokenIdStart;
    public string[] nftContracts = {};

    async void Start()
    {
        // This is the account taken from the user login scene
        account = PlayerPrefs.GetString("Account");
        // Searches through your listed contracts
        foreach (string contract in nftContracts)
        {
            int balance = await ERC721.BalanceOf(chain, network, contract, account);
            Debug.Log("Balance of contract " + contract + ": " + balance);
            // if i is less than the selected amount of tokenIDs to search, keep searching
            for (int i = 1; i <amountOfTokenIdsToSearch; i++)
            {
                // if balanceSearched is less than the balance for each contract, keep searching
                if (balanceSearched < balance)
                {
                    string ownerOf = await ERC721.OwnerOf(chain, network, contract, (tokenIdStart + i).ToString());
                    // if token id id matches the account from login, print the tokenID and get the URI
                    if (ownerOf == account)
                    {
                        string uri = await ERC721.URI(chain, network, contract, (tokenIdStart + i).ToString());
                        Debug.Log("TokenID: " + (tokenIdStart + i));
                        Debug.Log("Token URI: " + uri);
                        balanceSearched ++;
                    }
                }
            }
        }
    }
}
