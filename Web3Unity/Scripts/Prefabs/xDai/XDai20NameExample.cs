using UnityEngine;

public class XDai20NameExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0xa106739de31fa7a9df4a93c9bea3e1bade0924e2";

        string name = await XDai20.Name(contract);

        print (name);
    }
}
