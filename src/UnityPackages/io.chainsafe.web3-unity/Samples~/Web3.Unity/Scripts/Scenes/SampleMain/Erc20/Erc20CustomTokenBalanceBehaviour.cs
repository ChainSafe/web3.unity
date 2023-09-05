using System.Threading.Tasks;

namespace Samples.Behaviours
{
    public class Erc20CustomTokenBalanceBehaviour : SampleBehaviour
    {
        public string contractAbi;
        public string contractAddress;

        private Erc20Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc20Sample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var result = await logic.CustomTokenBalance(contractAbi, contractAddress);
            SampleOutputUtil.PrintResult(result, nameof(Erc20Sample), nameof(Erc20Sample.CustomTokenBalance));
        }
    }
}