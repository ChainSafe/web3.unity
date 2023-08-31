using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class Sha3Behaviour : SampleBehaviour
    {
        public string message = "It’s dangerous to go alone, take this!";

        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override Task ExecuteSample()
        {
            var hash = logic.Sha3(message);

            SampleOutputUtil.PrintResult(hash, nameof(UnsortedSample), nameof(UnsortedSample.Sha3));
            return Task.CompletedTask;
        }
    }
}