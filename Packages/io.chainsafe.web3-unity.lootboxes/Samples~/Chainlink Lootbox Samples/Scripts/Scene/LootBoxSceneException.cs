using System;

namespace Chainlink.LootBoxes.Scene
{
    public class LootBoxSceneException : Exception
    {
        public LootBoxSceneException(string message, Exception originalException = null)
            : base(message, originalException)
        {
        }
    }
}