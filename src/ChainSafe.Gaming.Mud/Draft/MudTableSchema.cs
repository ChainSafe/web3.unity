using System.Collections.Generic;
using Nethereum.Mud.EncodingDecoding;

namespace ChainSafe.Gaming.Mud.Draft
{
    public class MudTableSchema
    {
        public string Namespace { get; set; }

        public string TableName { get; set; }

        /// <summary>
        /// Column's EVM data type by name.
        /// </summary>
        public Dictionary<string, string> Columns { get; set; }

        public string[] KeyColumns { get; set; }

        public bool IsOffChain { get; set; }

        public byte[] ResourceId => MudUtils.TableResourceId(Namespace, TableName, IsOffChain);
    }
}