using System.Numerics;
using UnityEngine;

public class ERC20TotalSupplyExample : MonoBehaviour
{
    async void Start()
    {
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string contract = "0xc778417e063141139fce010982780140aa0cd5ab";

        BigInteger totalSupply = await ERC20.TotalSupply(network, contract);

        print (totalSupply);
    }
}
