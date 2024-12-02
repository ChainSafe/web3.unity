using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using UnityEngine;

public class AutoConnect : MonoBehaviour
{
    private void Awake()
    {
        Web3Unity.Web3Initialized += Web3Initialized;
    }

    private void Web3Initialized((Web3 web3, bool isLightweight) valueTuple)
    {
        if (valueTuple.isLightweight)
        {
            Web3Unity.ConnectionScreen.Open();
        }
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        Web3Unity.Web3Initialized -= Web3Initialized;
    }
}
