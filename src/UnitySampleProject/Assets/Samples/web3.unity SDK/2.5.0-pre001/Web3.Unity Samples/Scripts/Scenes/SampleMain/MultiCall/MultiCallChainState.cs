using System.Threading.Tasks;
using UnityEngine;

namespace Samples.Behaviours.MultiCall
{
    public class MultiCallChainState : SampleBehaviour
    {
        private MultiCallSample _logic;

        protected override void Awake()
        {
            base.Awake();
            _logic = new MultiCallSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            Debug.Log("Starting");
            var result = await _logic.BlockStateExample();
            Debug.Log(result.Length.ToString());
            Debug.Log($"Success: {result[0].Success}");
            Debug.Log($"Value: {result[0].Value.ToString()}");
            SampleOutputUtil.PrintResult(result[0].Value.ToString(), nameof(MultiCallSample), nameof(MultiCallSample.BlockStateExample));
        }
    }
}