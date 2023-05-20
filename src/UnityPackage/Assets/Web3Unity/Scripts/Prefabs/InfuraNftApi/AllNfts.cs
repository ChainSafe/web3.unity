using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AllNfts : MonoBehaviour
{
    /*
    Infura has come out swinging with a potential replacement for blockchain indexing.

    Link: https://docs.infura.io/infura/infura-expansion-apis/nft-api

    The included script will include everything you need to start shaping your own custom API calls to their endpoints and query data like:

    Transfers
    Ownership
    Metadata
    Collections
    Market Data
    In the included example, we query all the owners for a specific token on the Cronos testnet.
     */
    
    // Feel free to change all of this. It's purely educational and nothing more than a proof of concept
    private string APICall;
    
    // Add your Infura API Key and Secret <API KEY>:<API SECRET>
    private string APIKey;

    // Which Chain is your contract deployed on?
    public string ChainID;

    // What is your NFT Contract Address?
    public string TokenContract;

    // What tokenID do you want to check against?
    public string TokenID;


    [Serializable]
    public class TokenData
    {
        public Owners[] owners;
    }

    [Serializable]
    public class Owners
    {
        public string ownerOf { get; set; }
        public string tokenAddress { get; set; }

        // You can add more key:value pairs here
    }


    public void Start()
    {
        // Add your Infura API Key and Secret
        APIKey = "<API KEY>:<API SECRET>";

        // https://docs.infura.io/infura/infura-expansion-apis/nft-api/rest-apis/api-reference
        // The below URL will call all the owners of a specific NFT on a specified network and contract.
        APICall = "https://nft.api.infura.io/networks/" + ChainID + "/nfts/" + TokenContract + "/" + TokenID + "/owners";

        // Invoke the coroutine
        StartCoroutine(CallInfura(APICall));
    }

    IEnumerator CallInfura(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            var APIAuth = "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(APIKey));

            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", APIAuth);

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);

                    // Deserialize our JSON object
                    TokenData tokens = JsonConvert.DeserializeObject<TokenData>(webRequest.downloadHandler.text);

                    // Iterate through all the owners within the Owner class
                    foreach (Owners owner in tokens.owners)
                    {
                        // Debug the wallets that own this
                        Debug.Log("Owner Wallet: " + owner.ownerOf);

                        // Code can be executed here to validate if the connected wallet is one of the listed wallets.
                    }
                    break;
            }
        }
    }
}