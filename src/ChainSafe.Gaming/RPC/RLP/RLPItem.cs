namespace ChainSafe.Gaming.Evm.RLP
{
    /// <summary>
    /// Represents an Item that conforms to the Recursive Length Prefix (RLP) encoding scheme.
    /// </summary>
    public class RLPItem : IRLPElement
    {
        private readonly byte[] rlpData;

        /// <summary>
        /// Initializes a new instance of the <see cref="RLPItem"/> class with the provided data.
        /// </summary>
        /// <param name="rlpData">The data to be encoded by RLP.</param>
        public RLPItem(byte[] rlpData)
        {
            this.rlpData = rlpData;
        }

        /// <summary>
        /// Gets the RLP encoded data.
        /// </summary>
        /// <returns>The encoded data as a byte array.</returns>
        public byte[] RLPData => GetRLPData();

        /// <summary>
        /// Returns the encoded RLP data.
        /// </summary>
        /// <returns>A byte array of the RLP encoded data. Returns null if the data is empty.</returns>
        private byte[] GetRLPData()
        {
            return rlpData.Length == 0 ? null : rlpData;
        }
    }
}