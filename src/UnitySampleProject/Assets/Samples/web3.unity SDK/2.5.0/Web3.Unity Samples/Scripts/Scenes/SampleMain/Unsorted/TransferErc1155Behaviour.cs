using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class TransferErc1155Behaviour : SampleBehaviour
    {
        public string toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        public string contractAddress = "0xA0a53f1Cabf7D723Ab2087400681039917D1B6D4";
        public int tokenId = 1;
        public int amount = 1;

        private UnsortedSample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new UnsortedSample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var response = await logic.TransferErc1155(contractAddress, tokenId, amount, toAccount);

            var output = SampleOutputUtil.BuildOutputValue(response);
            SampleOutputUtil.PrintResult(output, nameof(UnsortedSample), nameof(UnsortedSample.TransferErc1155));
        }
    }
}