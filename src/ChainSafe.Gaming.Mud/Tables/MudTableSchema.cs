using System;
using System.Collections.Generic;
using System.Linq;

namespace ChainSafe.Gaming.Mud.Tables
{
    public class MudTableSchema
    {
        public string Namespace { get; set; }

        public string TableName { get; set; }

        /// <summary>
        /// Column's EVM data type by name.
        /// </summary>
        public List<KeyValuePair<string, string>> Columns { get; set; }

        public string[] KeyColumns { get; set; } = Array.Empty<string>();

        public bool IsOffChain { get; set; }

        public byte[] ResourceId => MudUtils.TableResourceId(Namespace, TableName, IsOffChain); // todo cache

        public string GetColumnType(string columnName) => Columns.Single(pair1 => pair1.Key == columnName).Value;

        public IEnumerable<int> FindKeyIndices() // todo cache
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