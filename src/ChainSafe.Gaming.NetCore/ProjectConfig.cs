using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.NetCore
{
    /// <summary>
    /// Implementation of <see cref="IProjectConfig"/> for NetCore environment.
    /// </summary>
    public class ProjectConfig : IProjectConfig
    {
        /// <summary>
        /// Implementation of <see cref="IProjectConfig.ProjectId"/>
        /// Project Id fetched from the ChainSafe Gaming web dashboard.
        /// </summary>
        public string ProjectId { get; set; }
    }
}