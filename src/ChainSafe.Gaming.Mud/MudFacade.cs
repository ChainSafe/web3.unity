using System.Diagnostics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Mud.Worlds;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Mud
{
    /// <summary>
    /// A facade class for all the MUD-related functionality.
    /// </summary>
    public class MudFacade
    {
        private readonly MudWorldFactory worldFactory;
        private readonly ILogWriter logWriter;

        public MudFacade(MudWorldFactory worldFactory, ILogWriter logWriter)
        {
            this.logWriter = logWriter;
            this.worldFactory = worldFactory;
        }

        /// <summary>
        /// Builds a new MudWorld client based on the provided configuration.
        /// </summary>
        /// <param name="worldConfig">The configuration settings for the world.</param>
        /// <returns>A Task that represents the asynchronous operation. The Task's result is the created MudWorld.</returns>
        public Task<MudWorld> BuildWorld(IMudWorldConfig worldConfig)
        {
            var stopwatch = Stopwatch.StartNew();
            var world = worldFactory.Build(worldConfig);
            logWriter.Log($"Loaded MUD world {worldConfig.ContractAddress} in {stopwatch.Elapsed}");
            return world;
        }
    }
}