using System;

namespace LootBoxes.Chainlink.Scene
{
    public class LootBoxSceneException : Exception
    {
        public LootBoxSceneException(string message, Exception originalException = null)
            : base(message, originalException)
        {
        }
    }
}