using UnityEngine;

public class SignMessage : MonoBehaviour
{
    public async void Start()
    {
        string response = await Web3Accessor.Web3.Signer.SignMessage("Hello from CS gaming!");
        Debug.Log(response);
    }
}
