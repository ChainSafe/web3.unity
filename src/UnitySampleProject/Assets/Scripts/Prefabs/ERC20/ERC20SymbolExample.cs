using ChainSafe.Gaming.UnityPackage.Ethereum.Eip;
using UnityEngine;

public class ERC20SymbolExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";

        string symbol = await ERC20.Symbol(contract);
        print(symbol);
    }
}
