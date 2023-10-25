using System.Collections.Generic;

namespace ChainSafe.Gaming.Evm.RLP
{
    /// <summary>
    /// Defines a Run-Length-Prefix (RLP) Collection.
    /// </summary>
    public class RLPCollection : List<IRLPElement>, IRLPElement
    {
        /// <summary>
        /// Gets or sets the RLP data.
        /// </summary>
        /// <value>
        /// A byte array containing the RLP data.
        /// </value>
        public byte[] RLPData { get; set; }
    }
}