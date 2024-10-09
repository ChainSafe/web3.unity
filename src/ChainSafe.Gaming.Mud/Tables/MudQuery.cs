namespace ChainSafe.Gaming.Mud.Tables
{
    /// <summary>
    /// This class represents a query for filtering records of a MUD table.
    /// </summary>
    public class MudQuery // todo extend to support complex filters
    {
        private MudQuery()
        {
        }

        /// <summary>
        /// A query that doesn't filter any records.
        /// </summary>
        public static MudQuery All { get; } = new();

        /// <summary>
        /// Should this Query simply look for a record using record key.
        /// </summary>
        public bool FindWithKey { get; private set; }

        /// <summary>
        /// The key to be used when looking for a record by it's key.
        /// </summary>
        public object[] KeyFilter { get; private set; }

        /// <summary>
        /// Creates a new instance of MudQuery that looks for a specific record by it's key.
        /// </summary>
        /// <param name="key">The key used to find the record.</param>
        /// <returns>A new instance of MudQuery with FindWithKey set to true and KeyFilter set to the specified key.</returns>
        public static MudQuery ByKey(object[] key)
        {
            return new MudQuery
            {
                FindWithKey = true,
                KeyFilter = key,
            };
        }
    }
}