using ChainSafe.Gaming.UnityPackage.Connection;

namespace ChainSafe.Gaming.Reown
{
    public static class ReownConfigExtensions
    {
        /// <summary>
        /// Sets <see cref="ReownConnectionProvider.ForceNewSession"/> property of this config object.
        /// </summary>
        /// <param name="provider">The config object.</param>
        /// <param name="forceNewSession">New value for ForceNewSession property.</param>
        /// <returns>Updated <see cref="ReownConnectionProvider"/> object.</returns>
        public static ReownConnectionProvider WithForceNewSession(this ReownConnectionProvider provider, bool forceNewSession)
        {
            provider.ForceNewSession = forceNewSession;
            return provider;
        }
    }
}