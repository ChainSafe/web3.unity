using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class SignMessageBehaviour : SampleBehaviour
    {
        public string message = "The right man in the wrong place can make all the difference in the world.";

        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var signedMessage = await logic.SignMessage(message);

            SampleOutputUtil.PrintResult(signedMessage, nameof(UnsortedSample), nameof(UnsortedSample.SignMessage));
        }
    }
}