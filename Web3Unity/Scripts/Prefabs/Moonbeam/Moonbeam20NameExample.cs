using UnityEngine;

public class Moonbeam20NameExample : MonoBehaviour
{
    async void Start()
    {
        string network = "testnet"; // testnet 
        string contract = "0xbDF2d708c6E4705824dC024187cd219da41C8c81";

        string name = await Moonbeam20.Name(network, contract);

        print (name);
    }
}
