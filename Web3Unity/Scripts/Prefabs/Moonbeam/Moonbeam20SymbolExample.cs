using UnityEngine;

public class Moonbeam20SymbolExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // testnet 
        string contract = "0xbDF2d708c6E4705824dC024187cd219da41C8c81";

        string symbol = await Moonbeam20.Symbol(network, contract);

        print (symbol);
    }
}
