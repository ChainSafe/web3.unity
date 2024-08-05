using ChainSafe.Gaming.Web3.Core.Nethereum;

namespace ChainSafe.Gaming.Mud
{
    public class MudWorldFactory
    {
        private readonly INethereumWeb3Adapter nethWeb3;

        public MudWorldFactory(INethereumWeb3Adapter nethWeb3)
        {
            this.nethWeb3 = nethWeb3;
        }

        public MudWorld Build(string worldAddress)
        {
            return new MudWorld(nethWeb3, worldAddress);
        }
    }
}