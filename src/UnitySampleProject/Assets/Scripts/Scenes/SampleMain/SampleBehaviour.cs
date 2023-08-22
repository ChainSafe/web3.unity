using System.Threading.Tasks;
using ChainSafe.GamingWeb3;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.Behaviours
{
    [RequireComponent(typeof(Button))]
    public abstract class SampleBehaviour : MonoBehaviour
    {
        protected Web3 Web3 => Web3Accessor.Web3;

        protected virtual void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Execute);
        }

        private async void Execute()
        {
            SampleFeedback.Instance?.Activate();

            try
            {
                await Task.Yield();
                await Task.WhenAll(ExecuteSample());
            }
            finally
            {
                SampleFeedback.Instance?.Deactivate();
            }
        }

        protected abstract Task ExecuteSample();
    }
}