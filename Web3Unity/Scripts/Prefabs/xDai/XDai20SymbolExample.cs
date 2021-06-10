using UnityEngine;

public class XDai20SymbolExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0xa106739de31fa7a9df4a93c9bea3e1bade0924e2";

        string symbol = await XDai20.Symbol(contract);

        print (symbol);
    }
}
