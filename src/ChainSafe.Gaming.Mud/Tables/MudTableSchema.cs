using System;
using System.Collections.Generic;
using System.Linq;

namespace ChainSafe.Gaming.Mud.Tables
{
    public class MudTableSchema
    {
        private byte[]? resourceId;
        private int[]? keyIndices;

        public string Namespace { get; set; }

        public string TableName { get; set; }

        /// <summary>
        /// EVM data type by name for each column in the table.
        /// </summary>
        public List<KeyValuePair<string, string>> Columns { get; set; } // this is not Dictionary because we care about the order

        public string[] KeyColumns { get; set; } = Array.Empty<string>();

        public bool IsOffChain { get; set; }

        public byte[] ResourceId => resourceId ??= MudUtils.TableResourceId(Namespace, TableName, IsOffChain);

        public int[] KeyIndices => keyIndices ??= FindKeyIndices().ToArray();

        public string GetColumnType(string columnName) => Columns.Single(pair => pair.Key == columnName).Value;

        private IEnumerable<int> FindKeyIndices()
        {
            if (KeyColumns.Length == 0)
            {
                yield break;
            }

            for (var i = 0; i < Columns.Count; i++)
            {
                if (KeyColumns.Contains(Columns[i].Key))
                {
                    yield return i;
                }
            }
        }
    }
}