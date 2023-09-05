using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class GetNonceBehaviour : SampleBehaviour
    {
        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var nonce = await logic.GetNonce();

            SampleOutputUtil.PrintResult(nonce.ToString(), nameof(UnsortedSample), nameof(UnsortedSample.GetNonce));
        }
    }
}