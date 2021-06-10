using System.Numerics;
using UnityEngine;

public class ERC1155BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string network = "rinkeby"; // mainnet ropsten kovan rinkeby goerli
        string contract = "0x2ebecabbbe8a8c629b99ab23ed154d74cd5d4342";
        string account = "0xaca9b6d9b1636d99156bb12825c75de1e5a58870";
        string tokenId = "17";

        BigInteger balance = await ERC1155.BalanceOf(network, contract, account, tokenId);

        print(balance);
    }
}
