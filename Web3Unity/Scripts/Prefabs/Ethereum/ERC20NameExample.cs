using UnityEngine;

public class ERC20NameExample : MonoBehaviour
{
    async void Start()
    {
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string contract = "0xc778417E063141139Fce010982780140Aa0cD5Ab";

        string name = await ERC20.Name(network, contract);

        print (name);
    }
}
