using System.Numerics;
using UnityEngine;

public class ERC20DecimalsExample : MonoBehaviour
{
    async void Start()
    {
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string contract = "0xc778417E063141139Fce010982780140Aa0cD5Ab";

        BigInteger decimals = await ERC20.Decimals(network, contract);

        print (decimals);
    }
}
