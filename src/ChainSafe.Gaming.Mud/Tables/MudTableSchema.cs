using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ChainSafe.Gaming.Mud.Tables
{
    /// <summary>
    /// Represents a schema for a table in the MUD world.
    /// </summary>
    public class MudTableSchema
    {
        [MaybeNull]
        private byte[] resourceId;

        [MaybeNull]
        private int[] keyIndices;

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>
        /// The namespace of the table.
        /// </value>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get; set; }

        /// <summary>
        /// EVM <b>data type (value)</b> by <b>name (key)</b>, for each column in the table.
        /// </summary>
        /// <remarks>Please note that the order is taken into account and must match the order of the columns in your table.</remarks>
        public List<KeyValuePair<string, string>> Columns { get; set; } // this is not Dictionary because we care about the order

        /// <summary>
        /// Gets or sets the key column names.
        /// </summary>
        /// <value>
        /// The key columns names.
        /// </value>
        public string[] KeyColumns { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Gets or sets a value indicating whether the object is off-chain.
        /// </summary>
        /// <value>
        /// <i>true</i> if the object is off-chain; otherwise, <i>false</i>.
        /// </value>
        public bool IsOffChain { get; set; }

        /// <summary>
        /// Retrieves the resource ID for a given resource.
        /// </summary>
        /// <value>
        /// The resource ID as a byte array.
        /// </value>
        public byte[] ResourceId => resourceId ??= MudUtils.TableResourceId(Namespace, TableName, IsOffChain);

        /// <summary>
        /// Gets the indices of the key columns.
        /// </summary>
        /// <remarks>
        /// The indices are calculated lazily upon first access and then cached for subsequent access.
        /// </remarks>
        public int[] KeyIndices => keyIndices ??= FindKeyIndices().ToArray();

        /// <summary>
        /// Retrieves the column type for the specified column name.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The column type.</returns>
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