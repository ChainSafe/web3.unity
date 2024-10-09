using System.Text.RegularExpressions;

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
            string regexPattern = @"^0x[a-fA-F0-9]{40}$";

            return !string.IsNullOrEmpty(value) && Regex.IsMatch(value, regexPattern);
        }

        /// <summary>
        /// Assert that the string provided is a public address.
        /// </summary>
        /// <param name="value">String to check.</param>
        /// <returns>String that was checked.</returns>
        /// <exception cref="Web3Exception">The string provided is not public address.</exception>
        public static string AssertIsPublicAddress(this string value)
        {
            if (!IsPublicAddress(value))
            {
                throw new Web3AssertionException($"\"{value}\" is not a public address.");
            }

            return value;
        }
    }
}