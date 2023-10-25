namespace ChainSafe.Gaming.Web3
{
    /// <summary>
    /// <see cref="IProjectConfig"/> merged with <see cref="IChainConfig"/>.
    /// </summary>
    public interface ICompleteProjectConfig : IProjectConfig, IChainConfig
    {
    }
}