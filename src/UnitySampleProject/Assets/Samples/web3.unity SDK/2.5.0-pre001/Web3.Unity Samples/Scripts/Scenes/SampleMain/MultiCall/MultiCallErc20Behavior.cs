using System.Threading.Tasks;

namespace Samples.Behaviours
{
    public class MultiCallErc20Behavior : SampleBehaviour
    {
        private MultiCallSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new MultiCallSample(Web3);
        }
        protected override async Task ExecuteSample()
        {
            await logic.ErcSamples();
        }
    }
}