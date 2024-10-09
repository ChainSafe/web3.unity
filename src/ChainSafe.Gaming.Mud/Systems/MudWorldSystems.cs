using ChainSafe.Gaming.Evm.Contracts;

namespace ChainSafe.Gaming.Mud.Systems
{
    public class MudWorldSystems
    {
        private readonly Contract contract;

        public MudWorldSystems(Contract contract)
        {
            this.contract = contract;
        }

        public MudSystems GetSystemsForNamespace(string @namespace)
        {
            return new MudSystems(@namespace, contract);
        }
    }
}