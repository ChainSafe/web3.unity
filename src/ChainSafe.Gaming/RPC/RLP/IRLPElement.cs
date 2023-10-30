namespace ChainSafe.Gaming.Evm.RLP
{
    /// <summary>
    ///     Wrapper class for decoded elements from an RLP encoded byte array.
    /// </summary>
    public interface IRLPElement
    {
        /// <summary>
        /// Gets the RLP data.
        /// </summary>
        /// <value>The RLP data.</value>
        byte[] RLPData { get; }
    }
}