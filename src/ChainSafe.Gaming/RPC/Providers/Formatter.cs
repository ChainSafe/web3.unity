namespace ChainSafe.Gaming.Evm.Providers
{
    /// <summary>
    /// Provides functionalities related to formatting operations.
    /// </summary>
    public class Formatter
    {
        /// <summary>
        /// Creates and gets an instance of Transactions.Formatter class.
        /// </summary>
        /// <returns>
        /// A Transactions.Formatter class instance.
        /// </returns>
        public Transactions.Formatter Transaction { get; } = new Transactions.Formatter();
    }
}