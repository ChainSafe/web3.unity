using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllErc721Example : MonoBehaviour
{
    async void Start()
    {
        string chain = "ethereum";
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string account = "0xebc0e6232fb9d494060acf580105108444f7c696";
        string contract = "";
        string balance = await EVM.AllErc721(chain, network, account, contract);
        print(balance);
    }
}
