using UnityEngine;

public class Sha3 : MonoBehaviour
{
    void Start()
    {
        string message = "hello";
        string hashedMessage = new Nethereum.Util.Sha3Keccack().CalculateHash(message);
        Debug.Log(hashedMessage);
    }
}
