namespace ChainSafe.Gaming.Web3.Core.Debug
{
    public static class AddressExtensions
    {
        /// <summary>
        /// Check if the string provided is a public address.
        /// </summary>
        /// <param name="value">String to check.</param>
        /// <returns>True if the string provided is a public address.</returns>
        public static bool IsPublicAddress(string value)
        {
            // TODO: more accurate test/Regex
            return !string.IsNullOrEmpty(value) && value.Length == 42;
        }

        /// <summary>
        /// Assert that the string provided is a public address.
        /// </summary>
        /// <param name="value">String to check.</param>
        /// <param name="variableName">Name of the string variable (nameof).</param>
        /// <returns>String that was checked.</returns>
        /// <exception cref="Web3Exception">The string provided is not public address.</exception>
        public static string AssertIsPublicAddress(this string value, string variableName)
        {
            if (!IsPublicAddress(value))
            {
                throw new Web3AssertionException($"\"{variableName}\" is not public address");
            }

            return value;
        }
    }
}