using System.Collections.Generic;

namespace ChainSafe.GamingWeb3.Evm.RLP
{
    public class RLPCollection : List<IRLPElement>, IRLPElement
    {
        public byte[] RLPData { get; set; }
    }
}