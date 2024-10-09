using ChainSafe.Gaming.Web3.Core.Chains;

namespace ChainSafe.Gaming.Web3
{
    /// <summary>
    /// <see cref="IProjectConfig"/> merged with <see cref="IChainConfig"/>.
    /// </summary>
    public interface ICompleteProjectConfig : IProjectConfig, IChainConfigSet
    {
    }
}