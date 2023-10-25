namespace ChainSafe.Gaming.Evm.HDNode.Wordlists
{
    /// <summary>
    /// Base class to represent a wordlist.
    /// </summary>
    public abstract class Wordlist
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Wordlist"/> class.
        /// </summary>
        /// <param name="local">local to set.</param>
        protected Wordlist(string local)
        {
            Local = local;
        }

        /// <summary>
        /// Gets the locale for this wordlist.
        /// </summary>
        public string Local { get; }

        /// <summary>
        /// Get word from word index.
        /// </summary>
        /// <param name="index">Index to get word from.</param>
        /// <returns>Word at the provided index.</returns>
        public abstract string GetWord(int index);

        /// <summary>
        /// Get index from the provided word in the wordlist.
        /// </summary>
        /// <param name="word">Word to get index for.</param>
        /// <returns>The index of the provided word.</returns>
        public abstract int GetWordIndex(string word);
    }
}