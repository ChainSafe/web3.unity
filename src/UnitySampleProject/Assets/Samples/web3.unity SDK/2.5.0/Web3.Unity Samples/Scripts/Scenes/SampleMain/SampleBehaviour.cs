using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.Behaviours
{
    [RequireComponent(typeof(Button))]
    public class SampleBehaviour : MonoBehaviour
    {
        private const string DefaultChainId = "5";

        public async void Execute()
        {
            // Activates the loading pop up to stop duplicate calls
            SampleFeedback.Instance?.Activate();

            // check if we're on default sample chain
            if (Web3Accessor.Web3.ChainConfig.ChainId != DefaultChainId)
            {
                // log error not exception to not break flow
                Debug.LogError($"Samples are configured for Chain Id {DefaultChainId}, Please Change Chain Id in Window > ChainSafe SDK > Server Settings to {DefaultChainId}");
            }
            
            // Deactivates the loading pop up after a few seconds
            await new WaitForSeconds(2);
            SampleFeedback.Instance?.Deactivate();
        }
    }
}