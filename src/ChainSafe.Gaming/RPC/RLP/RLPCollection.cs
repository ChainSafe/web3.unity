using System.Collections.Generic;

namespace ChainSafe.Gaming.Evm.RLP
{
    public class RLPCollection : List<IRLPElement>, IRLPElement
    {
        public byte[] RLPData { get; set; }
    }
}