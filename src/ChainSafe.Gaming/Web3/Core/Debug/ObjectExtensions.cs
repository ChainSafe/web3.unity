namespace ChainSafe.Gaming.Web3.Core.Debug
{
    public static class ObjectExtensions
    {
#nullable enable
        /// <summary>
        /// Asserts that the object provided is not null.
        /// </summary>
        /// <param name="obj">Object to check.</param>
        /// <param name="variableName">Name of the variable to check (nameof).</param>
        /// <typeparam name="T">Type of the variable to check.</typeparam>
        /// <returns>The object provided.</returns>
        /// <exception cref="Web3AssertionException">The provided object is null.</exception>
        public static T AssertNotNull<T>(this T? obj, string variableName)
            where T : notnull
        {
            if (obj is null)
            {
                throw new Web3AssertionException($"{variableName} is null.");
            }

            return obj;
        }
#nullable disable
    }
}