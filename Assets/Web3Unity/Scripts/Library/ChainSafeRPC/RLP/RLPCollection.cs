using System.Collections.Generic;

namespace Web3Unity.Scripts.Library.Ethers.RLP
{
    public class RLPCollection : List<IRLPElement>, IRLPElement
    {
        public byte[] RLPData { get; set; }
    }
}