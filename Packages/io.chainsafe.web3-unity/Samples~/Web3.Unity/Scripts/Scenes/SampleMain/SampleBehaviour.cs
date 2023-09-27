using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
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

                try
                {
                    await Task.WhenAll(ExecuteSample());
                }

                catch (Exception e)
                {
                    Debug.LogError(e);

                    SampleFeedback.Instance?.ShowMessage($"{e.Message} : check console for more detail", Color.red, 5f);
                }
            }

            finally
            {
                SampleFeedback.Instance?.Deactivate();
            }
        }

        protected abstract Task ExecuteSample();
    }
}