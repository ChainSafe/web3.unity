using System.Numerics;
using UnityEngine;

public class ERC20BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string contract = "0xc778417e063141139fce010982780140aa0cd5ab";
        string account = "0xaCA9B6D9B1636D99156bB12825c75De1E5a58870";

        BigInteger balance = await ERC20.BalanceOf(network, contract, account);
        
        print (balance);
    }
}
