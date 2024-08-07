using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Web3.Core.Nethereum;

namespace ChainSafe.Gaming.Mud
{
    public class MudWorldFactory
    {
        private readonly INethereumWeb3Adapter nethWeb3;
        private IContractBuilder contractBuilder;

        public MudWorldFactory(INethereumWeb3Adapter nethWeb3, IContractBuilder contractBuilder)
        {
            this.contractBuilder = contractBuilder;
            this.nethWeb3 = nethWeb3;
        }

        public MudWorld Build(string worldAddress, string worldContractAbi)
        {
            var contract = contractBuilder.Build(worldContractAbi, worldAddress);
            var mudWorld = new MudWorld(nethWeb3, contract);
            return mudWorld;
        }
    }
}