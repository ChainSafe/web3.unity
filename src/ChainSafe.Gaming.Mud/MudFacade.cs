using System.Diagnostics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Mud.Worlds;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Mud
{
    public class MudFacade
    {
        private readonly MudWorldFactory worldFactory;
        private readonly ILogWriter logWriter;

        public MudFacade(MudWorldFactory worldFactory, ILogWriter logWriter)
        {
            this.logWriter = logWriter;
            this.worldFactory = worldFactory;
        }

        public Task<MudWorld> BuildWorld(MudWorldConfig worldConfig)
        {
            var stopwatch = Stopwatch.StartNew();
            var world = worldFactory.Build(worldConfig);
            logWriter.Log($"Loaded world {worldConfig.ContractAddress} in {stopwatch.Elapsed}");
            return world;
        }
    }
}