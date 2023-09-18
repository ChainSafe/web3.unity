using System.Threading.Tasks;

namespace Samples.Behaviours
{
    public class Erc20NativeBalanceOfBehaviour : SampleBehaviour
    {
        public string account = "0xaBed4239E4855E120fDA34aDBEABDd2911626BA1";

        private Erc20Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc20Sample(Web3);
        }

        protected override async Task ExecuteSample()
        {
            var result = await logic.NativeBalanceOf(account);
            SampleOutputUtil.PrintResult(result.ToString(), nameof(Erc20Sample), nameof(Erc20Sample.NativeBalanceOf));
        }
    }
}