using System.Threading.Tasks;

namespace Samples.Behaviours
{
    public class Erc20BalanceOfBehaviour : SampleBehaviour
    {
        public string contractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
        public string account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

        private Erc20Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc20Sample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var balance = await logic.BalanceOf(contractAddress, account);
            SampleOutputUtil.PrintResult(balance.ToString(), nameof(Erc20Sample), nameof(Erc20Sample.BalanceOf));
        }
    }
}