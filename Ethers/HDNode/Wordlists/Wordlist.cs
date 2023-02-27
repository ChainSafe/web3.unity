namespace Web3Unity.Scripts.Library.Ethers.HDNode.Wordlists
{
    public abstract class Wordlist
    {
        public readonly string Local;

        protected Wordlist(string local)
        {
            Local = local;
        }

        public abstract string GetWord(int index);
        public abstract int GetWordIndex(string word);
    }
}