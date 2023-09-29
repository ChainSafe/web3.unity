using System.Threading.Tasks;

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
            await _logic.BlockStateExample();
        }
    }
}