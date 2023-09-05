using System.Threading.Tasks;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Unsorted
{
    public class TransferErc1155Behaviour : SampleBehaviour
    {
        public string toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
        public string contractAddress = "0xae283E79a5361CF1077bf2638a1A953c872AD973";
        public int tokenId = 0;
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