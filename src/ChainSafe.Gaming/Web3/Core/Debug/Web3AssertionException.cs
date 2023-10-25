namespace ChainSafe.Gaming.Web3.Core.Debug
{
    /// <summary>
    /// Exception indicating that an assertion check failed.
    /// </summary>
    public class Web3AssertionException : Web3Exception
    {
        public Web3AssertionException(string message)
            : base(message)
        {
        }
    }
}