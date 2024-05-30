namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// Config for a HyperPlay connection.
    /// </summary>
    public class HyperPlayConfig : IHyperPlayConfig
    {
        /// <summary>
        /// Remember the HyperPlay session.
        /// Like remember me for login.
        /// </summary>
        public bool RememberSession { get; set; }
    }
}