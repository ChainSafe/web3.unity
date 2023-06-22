using System.Collections.Generic;

namespace ChainSafe.Gaming.Evm.Rlp
{
    public class RLPCollection : List<IRLPElement>, IRLPElement
    {
        public byte[] RLPData { get; set; }
    }
}