using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class AllErc721Example : MonoBehaviour
{
    private class NFTs
    {
        public string contract { get; set; }
        public string tokenId { get; set; }
        public string uri { get; set; }
        public string balance { get; set; }
    }

    async void Start()
    {
        string chain = "ethereum";
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string account = "0xebc0e6232fb9d494060acf580105108444f7c696";
        string contract = "";
        int first = 500;
        int skip = 0;
        string response = await EVM.AllErc721(chain, network, account, contract, first, skip);
        try
        {
            NFTs[] erc721s = JsonConvert.DeserializeObject<NFTs[]>(response);
            print(erc721s[0].contract);
            print(erc721s[0].tokenId);
            print(erc721s[0].uri);
            print(erc721s[0].balance);
        }
        catch
        {
           print("Error: " + response);
        }
    }
}
