using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Contracts;

public class ProviderEvent : MonoBehaviour
{
    private void Start()
    {
        Web3Accessor.Web3.Events.NewBlock += OnNewBlock;
    }

    private void OnDestroy()
    {
        // Web3Accessor.Web3 cannot be used here since it may create a new game object,
        // which cannot happen during OnDestroy
        var web3 = Web3Accessor.TryWeb3;
        if (web3 != null)
        {
            // Note: it is important to remove event handlers once you're done with them,
            // see https://stackoverflow.com/questions/298261/do-event-handlers-stop-garbage-collection-from-occurring
            // for more details
            web3.Events.NewBlock -= OnNewBlock;
        }
    }

    private void OnNewBlock(ulong blockNumber)
    {
        Debug.Log($"New block: {blockNumber}");
    }
}