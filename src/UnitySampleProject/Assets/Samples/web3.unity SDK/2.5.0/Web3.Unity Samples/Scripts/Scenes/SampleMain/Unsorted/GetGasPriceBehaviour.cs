using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class GetGasPriceBehaviour : SampleBehaviour
    {
        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var gasPrice = await logic.GetGasPrice();

            SampleOutputUtil.PrintResult(gasPrice.ToString(), nameof(UnsortedSample), nameof(UnsortedSample.GetGasPrice));
        }
    }
}