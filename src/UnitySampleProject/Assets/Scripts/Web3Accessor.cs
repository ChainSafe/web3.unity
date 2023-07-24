using ChainSafe.GamingWeb3;
using UnityEngine;

public class Web3Accessor : MonoBehaviour
{
    static Web3Accessor instance;

    Web3 web3;

    public static Web3Accessor Instance
    {
        get
        {
            if (!instance)
            {
                var go = new GameObject("Web3Accessor");
                DontDestroyOnLoad(go);
                instance = go.AddComponent<Web3Accessor>();
            }

            return instance;
        }
    }

    public static Web3 Web3 => Instance?.web3 ?? throw new System.Exception("Web3 instance not initialized");

    public void Set(Web3 web3)
    {
        if (this.web3 != null)
        {
            throw new System.Exception("Web3 instance was already initialized");
        }

        this.web3 = web3;
    }

    public void Clear() => web3 = null;
}
