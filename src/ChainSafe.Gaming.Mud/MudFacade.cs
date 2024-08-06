namespace ChainSafe.Gaming.Mud
{
    public class MudFacade
    {
        private readonly MudWorldFactory worldFactory;

        public MudFacade(MudWorldFactory worldFactory)
        {
            this.worldFactory = worldFactory;
        }

        /// <summary>
        /// Builds a MUD World Client to exchange messages with a World Contract.
        /// </summary>
        /// <param name="contractAddress">The address of the World Contract.</param>
        /// <param name="worldContractAbi">The ABI of the World Contract.</param>
        /// <returns>The client for the MUD World Contract.</returns>
        public MudWorld BuildWorld(string contractAddress, string worldContractAbi)
        {
            return worldFactory.Build(contractAddress, worldContractAbi);
        }
    }
}