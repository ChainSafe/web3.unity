using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class GetBlockNumberBehaviour : SampleBehaviour
    {
        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var blockNumber = await logic.GetBlockNumber();

            SampleOutputUtil.PrintResult(blockNumber.ToString(), nameof(UnsortedSample), nameof(UnsortedSample.GetBlockNumber));
        }
    }
}