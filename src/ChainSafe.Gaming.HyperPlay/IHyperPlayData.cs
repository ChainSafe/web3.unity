using ChainSafe.Gaming.LocalStorage;

namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// <see cref="IStorable"/> data for HyperPlay.
    /// Persisted data for HyperPlay.
    /// </summary>
    public interface IHyperPlayData : IStorable
    {
        /// <summary>
        /// Remember session from a previous connection.
        /// </summary>
        public bool RememberSession { get; set; }

        /// <summary>
        /// Saved account from a previous session.
        /// </summary>
        public string SavedAccount { get; set; }
    }
}