using System.Threading.Tasks;

namespace Samples.Behaviours
{
    public class Erc20TotalSupplyBehaviour : SampleBehaviour
    {
        public string contractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";

        private Erc20Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc20Sample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var result = await logic.TotalSupply(contractAddress);
            SampleOutputUtil.PrintResult(result.ToString(), nameof(Erc20Sample), nameof(Erc20Sample.TotalSupply));
        }
    }
}