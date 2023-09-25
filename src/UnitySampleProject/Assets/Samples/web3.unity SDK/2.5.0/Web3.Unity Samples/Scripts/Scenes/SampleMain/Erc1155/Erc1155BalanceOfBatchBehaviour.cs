using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using Web3Unity.Scripts.Prefabs;

namespace Samples.Behaviours.Erc1155
{
    public class Erc1155BalanceOfBatchBehaviour : SampleBehaviour
    {
        public string contractAddress = "0xdc4aff511e1b94677142a43df90f948f9ae181dd";
        public string[] accounts = { "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2", "0xE51995Cdb3b1c109E0e6E67ab5aB31CDdBB83E4a" };
        public string[] tokenIds = { "1", "2" };

        private Erc1155Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc1155Sample(Web3Accessor.Web3);
        }

        protected override async Task ExecuteSample()
        {
            // todo not sure what is result of this operation
            // users wouldn't get it also
            var balances = await logic.BalanceOfBatch(contractAddress, accounts, tokenIds);
            var balancesString = string.Join(", ", balances);
            SampleOutputUtil.PrintResult(balancesString, nameof(Erc1155Sample), nameof(Erc1155Sample.BalanceOfBatch));
        }
    }
}