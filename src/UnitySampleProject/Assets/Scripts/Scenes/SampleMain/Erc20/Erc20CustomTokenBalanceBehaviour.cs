namespace Samples.Behaviours
{
    public class Erc20CustomTokenBalanceBehaviour : ButtonBehaviour
    {
        public string contractAbi;
        public string contractAddress;
        
        private Erc20Sample logic;

        protected override void Awake()
        {
            base.Awake();
            logic = new Erc20Sample(Web3Accessor.Web3);
        }

        protected override async void Execute()
        {
            var result = await logic.CustomTokenBalance(contractAbi, contractAddress);
            SampleOutputUtil.PrintResult(result, nameof(Erc20Sample), nameof(Erc20Sample.CustomTokenBalance));
        }
    }
}