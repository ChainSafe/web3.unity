using UnityEngine;

public class ERC20SymbolExample : MonoBehaviour
{
    async void Start()
    {
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string contract = "0xc778417E063141139Fce010982780140Aa0cD5Ab";

        string symbol = await ERC20.Symbol(network, contract);

        print (symbol);
    }
}
