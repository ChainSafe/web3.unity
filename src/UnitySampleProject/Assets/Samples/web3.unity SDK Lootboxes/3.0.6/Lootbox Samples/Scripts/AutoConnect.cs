using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using UnityEngine;

public class AutoConnect : MonoBehaviour
{
    #region Methods

    /// <summary>
    /// Subscribes to the web3 initialized event so it can react when called.
    /// </summary>
    private void Awake()
    {
        Web3Unity.Web3Initialized += Web3Initialized;
    }

    /// <summary>
    /// Executes when the web3 object is initialized, prompts the user to login if needed.
    /// </summary>
    /// <param name="valueTuple">Value tuple containing web3 object and lightweight bool.</param>
    private void Web3Initialized((Web3 web3, bool isLightweight) valueTuple)
    {
        if (valueTuple.isLightweight)
        {
            Web3Unity.ConnectionScreen.Open();
            Debug.Log("Web3 connection not found");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Web3 connection established");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Unsubscribes from the event to free up memory.
    /// </summary>
    public void OnDestroy()
    {
        Web3Unity.Web3Initialized -= Web3Initialized;
    }

    #endregion
}